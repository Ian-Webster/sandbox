namespace Messaging.Example.Business.Models
{
    public class HelloConsumer1Message : MessageBase
    {
        public HelloConsumer1Message(): base("Hello Consumer 1")
        {
        }
    }
}