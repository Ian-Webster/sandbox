using Messaging.Outbox.Data.Enums;
using Messaging.Outbox.Data.Repositories;
using Messaging.Outbox.Domain.Messages;
using Messaing.Shared.Business.Models;
using Messaing.Shared.Business.Producer;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace Messaging.Outbox.Producer
{
    public class KafkaProducerService : IHostedService
    {
        private readonly IMessageRepository _messageRepository;
        private KafkaProducer<OutboxMessageBase> _producer;

        public KafkaProducerService(IMessageRepository messageRepository)
        {
            _producer = new KafkaProducer<OutboxMessageBase>(
                new ProducerConfiguration
                {
                    KafkaHost = "localhost:9092",
                    ProducerGroupName = "OutboxProducer"
                }
            );
            _messageRepository = messageRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var unsentMessages = await _messageRepository.GetUnSentMessages(cancellationToken);
                if (!unsentMessages.Any()) 
                {
                    await Task.Delay(1000, cancellationToken);
                    continue;
                }

                unsentMessages.ToList().ForEach(async m =>
                {
                    var hydratedMessage = JsonSerializer.Deserialize<OutboxMessageBase>(m.MessageContent);
                    var sendResult = _producer.SendMessage(m.Topic, hydratedMessage, cancellationToken);

                    switch (sendResult.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            m.SentDate = DateTime.UtcNow;
                            m.Status = MessageStatus.Persisted;
                            break;
                        case TaskStatus.Faulted:
                            m.Error = sendResult.Exception.Message;
                            m.Status = MessageStatus.NotPersisted;
                            break;
                        case TaskStatus.Created:
                        case TaskStatus.WaitingForActivation:
                            m.Status = MessageStatus.PossiblyPersisted;
                            break;
                    }

                    await _messageRepository.UpdateMessage(m, cancellationToken);
                });
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
