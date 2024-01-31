using Confluent.Kafka;
using Messaging.Outbox.Domain.Messages;
using Messaing.Shared.Business.Consumer;
using Messaing.Shared.Business.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Spectre.Console;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Messaging.OutBox.Consumer1
{
    public class KafkaConsumerService : IHostedService
    {
        private KafkaConsumer<OutboxMessageBase> _consumer;

        public KafkaConsumerService()
        {
            _consumer = new KafkaConsumer<OutboxMessageBase>(
                new ConsumerConfiguration
                {
                    KafkaHost = "localhost:9092",
                    ConsumerGroupName = "OutboxConsumer1",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    SessionTimeout = 6000
                }
            );

            // _consumer.SubscribeToTopic("outbox");
            _consumer.SubscribeToTopics(
                new List<string> { "Outbox-HelloAll", "Outbox-HelloConsumer1" });
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                AnsiConsole.MarkupLine($"[bold purple]Testing {DateTime.UtcNow.ToLongTimeString()}[/]");
                var message = _consumer.ConsumeMessage();
                if (message != null && !message.IsPartitionEOF)
                {
                    AnsiConsole.MarkupLine($"[bold purple]Message received {message.Message.Value.MessageId}-{message.Offset}-{message.Message.Value.Message}[/]");
                    _consumer.CommitMessage(message);
                }
                await Task.Delay(200, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
