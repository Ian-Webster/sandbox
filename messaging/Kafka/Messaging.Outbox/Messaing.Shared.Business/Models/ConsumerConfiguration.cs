using Confluent.Kafka;

namespace Messaing.Shared.Business.Models
{
    public class ConsumerConfiguration
    {
        public string KafkaHost { get; set; }

        public string ConsumerGroupName { get; set; }

        public int MessageTimeout { get; set; }

        public int SessionTimeout { get; set; }

        public AutoOffsetReset AutoOffsetReset { get; set; }
    }
}
