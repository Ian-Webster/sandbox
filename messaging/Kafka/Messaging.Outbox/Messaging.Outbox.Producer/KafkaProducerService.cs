using Messaging.Outbox.Data.Enums;
using Messaging.Outbox.Data.Repositories;
using Messaging.Outbox.Domain.Messages;
using Messaing.Shared.Business.Models;
using Messaging.Shared.Business.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Messaging.Outbox.Producer
{
    public class KafkaProducerService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly KafkaProducer<OutboxMessageBase> _producer;

        public KafkaProducerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var logger = _serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<ILogger<KafkaProducer<OutboxMessageBase>>>();
            _producer = new KafkaProducer<OutboxMessageBase>(
                new ProducerConfiguration
                {
                    KafkaHost = "localhost:9092",
                    ProducerGroupName = "OutboxProducer"
                },
                logger
            );
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                if (_producer.IsFaulted)
                {
                    AnsiConsole.MarkupLine("[bold red]Kafka is down, waiting 30 seconds and trying again[/]");
                    if (!await _producer.KafkaIsUp())
                    {
                        // wait 30 seconds for the broker to be available
                        // if it's still not available, try again on the next loops
                        await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
                        continue;
                    }
                    // kafka must be up if we hit this point
                    
                }

                using var scope = _serviceScopeFactory.CreateScope();
                var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
                var unsentMessages = await messageRepository.GetUnSentMessages(cancellationToken);
                if (!unsentMessages.Any()) 
                {
                    await Task.Delay(1000, cancellationToken);
                    continue;
                }

                foreach (var m in unsentMessages)
                {
                    var hydratedMessage = JsonSerializer.Deserialize<OutboxMessageBase>(m.MessageContent);

                    var sendResult = _producer.SendMessage(m.Topic, hydratedMessage, cancellationToken);

                    if (sendResult == null)
                    {
                        m.Status = MessageStatus.NotSet;
                        m.Error = "Failed to send message to Kafka";
                        await messageRepository.UpdateMessage(m, cancellationToken);
                        AnsiConsole.MarkupLine($"[bold red]Kafka is down, waiting 30 seconds to retry[/]");
                        // wait 30 seconds for the broker to be available
                        await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
                        continue;
                    }

                    switch (sendResult.Status)
                    {
                        case TaskStatus.RanToCompletion:
                        case TaskStatus.WaitingForActivation:
                            m.SentDate = DateTime.UtcNow;
                            m.Status = MessageStatus.Persisted;
                            break;
                        case TaskStatus.Faulted:
                            m.Error = sendResult.Exception.Message;
                            m.Status = MessageStatus.NotPersisted;
                            break;
                        case TaskStatus.Created:
                            m.Status = MessageStatus.PossiblyPersisted;
                            break;
                    }

                    await messageRepository.UpdateMessage(m, cancellationToken);
                    
                };
                await Task.Delay(1000, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _producer.Dispose();
            return Task.CompletedTask;
        }
    }
}
