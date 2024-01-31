using System.Text.Json.Serialization;

namespace Messaging.Example.Business.Models
{
    public class MessageBase
    {
        protected Guid MessageId { get; set; }

        protected string Message { get; set; }

        protected DateTime Created { get; set; }

        protected MessageBase(string message)
        {
            MessageId = Guid.NewGuid();
            Created = DateTime.Now;
            Message = message;  
        }

        public override string ToString()
        {
            return $"{MessageId} - {Message} - {Created}";
        }
    }
}
