using Confluent.Kafka;

namespace Messaing.Shared.Business.Models
{
    /// <summary>
    /// Base class for messages
    /// </summary>
    public abstract class MessageBase
    {
        public Guid MessageId { get; set; }

        public DateTime CreatedAt { get; set; }

        protected virtual void Initialise()
        {
            // default initialisation
            MessageId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

    }
}
