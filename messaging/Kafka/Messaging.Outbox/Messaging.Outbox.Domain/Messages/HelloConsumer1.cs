namespace Messaging.Outbox.Domain.Messages
{
    public class HelloConsumer1: OutboxMessageBase
    {
        public HelloConsumer1()
        {
            Message = "Hello, consumer 1!";
        }
    }
}
