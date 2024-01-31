using Confluent.Kafka;
using Messaing.Shared.Business.Models;
using Messaing.Shared.Business.Serialisers;

namespace Messaing.Shared.Business.Consumer
{
    public class KafkaConsumer<TMessage> : IKafkaConsumer<TMessage>, IDisposable where TMessage : MessageBase
    {
        private IConsumer<string, TMessage> _consumer;

        public KafkaConsumer(ConsumerConfiguration consumerConfiguration)
        {
            _consumer = new ConsumerBuilder<string, TMessage>(
                new ConsumerConfig
                {
                    BootstrapServers = consumerConfiguration.KafkaHost,
                    GroupId = consumerConfiguration.ConsumerGroupName,
                    AutoOffsetReset = consumerConfiguration.AutoOffsetReset,
                    EnableAutoCommit = false,
                    EnablePartitionEof = true,
                    SessionTimeoutMs = consumerConfiguration.SessionTimeout
                })
                .SetValueDeserializer(new ByteArraySerialiser<TMessage>())
                .Build();
        }

        public void SubscribeToTopic(string topicName)
        {
            _consumer.Subscribe(topicName);  
        }

        public void SubscribeToTopics(ICollection<string> topicNames)
        {
            if (topicNames == null || topicNames.Count == 0)
            {
                throw new ArgumentException("No topics provided", nameof(topicNames));
            }

            _consumer.Subscribe(topicNames);
        }

        public ConsumeResult<string, TMessage> ConsumeMessage()
        {
            return _consumer.Consume();
        }

        public void CommitMessage(ConsumeResult<string, TMessage> message)
        {
            _consumer.Commit(message);
        }

        public void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
        }

        
    }
}
