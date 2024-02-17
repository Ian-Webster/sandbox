using Messaing.Shared.Business.Models;

namespace Messaging.Outbox.Domain.Messages
{
    public class OutboxMessageBase : MessageBase
    {
        public string Message { get; set; }

        public OutboxMessageBase()
        {
            Initialise();
        }

        public override string ToString()
        {
            return $"{MessageId} - {Message} - {CreatedAt}";
        }
    }
}
