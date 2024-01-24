namespace Messaging.Example.Business.Models
{
    public class MessageBase
    {
        public Guid MessageId { get; }

        public string Message { get; }

        public DateTime Created { get; }

        protected MessageBase(string message)
        {
            MessageId = Guid.NewGuid();
            Message = message;
            Created = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"{MessageId} - {Message} - {Created}";
        }
    }
}
