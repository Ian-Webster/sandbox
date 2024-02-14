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
                .SetErrorHandler((_, e) => { Subscribed = false; })
                .Build();
        }

        public bool Subscribed { get; set; }

        public bool SubscribeToTopic(string topicName)
        {
            try
            {
                _consumer.Subscribe(topicName);
                Subscribed = true;
                return true;
            }
            catch (Exception ex)
            {
                // log the exception
                return false;
            } 
        }

        public bool SubscribeToTopics(ICollection<string> topicNames)
        {
            if (topicNames == null || topicNames.Count == 0)
            {
                throw new ArgumentException("No topics provided", nameof(topicNames));
            }

            try
            {
                _consumer.Subscribe(topicNames);
                return true;
            }
            catch (Exception ex)
            {
                // log the exception
                return false;
            }
        }

        public ConsumeResult<string, TMessage>? ConsumeMessage(CancellationToken token)
        {
            try
            {
                return _consumer.Consume(token);
            }
            catch (Exception ex)
            {
                // log the exception
                return null;
            }
        }

        public bool CommitMessage(ConsumeResult<string, TMessage> message)
        {
            try
            {
                _consumer.Commit(message);
                return true;
            }
            catch (Exception ex)
            {
                // log the exception
                return false;
            }
        }

        public void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
        }

        
    }
}
