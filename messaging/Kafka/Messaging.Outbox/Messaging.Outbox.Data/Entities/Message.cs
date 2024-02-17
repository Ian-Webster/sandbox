using Messaging.Outbox.Data.Enums;

namespace Messaging.Outbox.Data.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }

        public string Topic { get; set; }

        public byte[] MessageContent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? SentDate { get; set; }

        public MessageStatus Status { get; set; }

        public string? Error { get; set; }
    }
}
