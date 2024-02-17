using System.Diagnostics;
using Messaging.Outbox.Consumer2.Services;
using Messaging.Outbox.Domain.Messages;
using Messaging.Shared.Business.Consumer;

namespace Messaging.Outbox.Consumer2;

public class KafkaConsumerService: BackgroundService
{
    private readonly IKafkaConsumer<OutboxMessageBase> _consumer;
    private readonly IMessageService _messageService;

    public KafkaConsumerService(IKafkaConsumer<OutboxMessageBase> consumer, IMessageService messageService)
    {
        _consumer = consumer;
        _messageService = messageService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_consumer.IsFaulted)
            {
                Console.WriteLine("Kafka is down, waiting 30 seconds and trying again");
                if (!await _consumer.KafkaIsUp())
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                    continue;
                }
            }

            if (!_consumer.IsSubscribed)
            {
                _consumer.SubscribeToTopics(new List<string> { "Outbox-HelloAll", "Outbox-HelloConsumer2" });
            }

            var message = _consumer.ConsumeMessage();
            if (message is { IsPartitionEOF: false })
            {
                Console.WriteLine($"Message received {message.Message.Value.MessageId}-{message.Offset}-{message.Message.Value.Message}[/]");
                _messageService.AddMessage(message.Message.Value);
                _consumer.CommitMessage(message);
            }

            await Task.Delay(200, stoppingToken);
        }
    }
}