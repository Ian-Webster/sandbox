namespace Messaging.Outbox.Domain.Messages
{
    public class HelloAll: OutboxMessageBase
    {
        public HelloAll() : base("Hello All")
        {
        }
    }
}
