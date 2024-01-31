namespace Messaging.Outbox.Domain.Messages
{
    public class HelloConsumer2: OutboxMessageBase
    {
        public HelloConsumer2() : base("Hello Consumer 2") { }
    }
}
