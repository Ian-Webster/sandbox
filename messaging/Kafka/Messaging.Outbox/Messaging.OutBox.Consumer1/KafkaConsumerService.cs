using Confluent.Kafka;
using Messaging.Outbox.Domain.Messages;
using Messaing.Shared.Business.Consumer;
using Messaing.Shared.Business.Models;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace Messaging.OutBox.Consumer1
{
    public class KafkaConsumerService : BackgroundService
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
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_consumer.Subscribed && !_consumer.SubscribeToTopics(
                        new List<string> { "Outbox-HelloAll", "Outbox-HelloConsumer1" }))
                {
                    // Kafka is down, wait for 30 seconds and try again on the next loop
                    AnsiConsole.MarkupLine("[bold red]Kafka is down, waiting 30 seconds and trying again[/]");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                var message = _consumer.ConsumeMessage(stoppingToken);
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
