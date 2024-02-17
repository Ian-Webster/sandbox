using Confluent.Kafka;
using Messaging.Outbox.Domain.Messages;
using Messaging.Shared.Business.Consumer;
using Messaing.Shared.Business.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace Messaging.OutBox.Consumer1
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly KafkaConsumer<OutboxMessageBase> _consumer;

        public KafkaConsumerService(IServiceScopeFactory serviceScope)
        {
            var scope = serviceScope.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<KafkaConsumer<OutboxMessageBase>>>();
            var config = scope.ServiceProvider.GetRequiredService<IOptions<ConsumerConfiguration>>();

            _consumer = new KafkaConsumer<OutboxMessageBase>(
                config,
                logger
            );
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_consumer.IsFaulted)
                {
                    AnsiConsole.MarkupLine("[bold red]Kafka is down, waiting 30 seconds and trying again[/]");
                    if (!await _consumer.KafkaIsUp())
                    {
                        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                        continue;
                    }
                }

                if (!_consumer.IsSubscribed)
                {
                    _consumer.SubscribeToTopics(new List<string> { "Outbox-HelloAll", "Outbox-HelloConsumer1" });
                }

                var message = _consumer.ConsumeMessage();
                if (message is { IsPartitionEOF: false })
                {
                    AnsiConsole.MarkupLine(
                        $"[bold purple]Message received {message.Message.Value.MessageId}-{message.Offset}-{message.Message.Value.Message}[/]");
                    _consumer.CommitMessage(message);
                }

                await Task.Delay(200, stoppingToken);
            }
        }
    }
}
