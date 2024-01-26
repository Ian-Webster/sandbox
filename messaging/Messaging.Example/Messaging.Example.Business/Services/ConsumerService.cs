using Confluent.Kafka;
using Messaging.Example.Business.Models;

namespace Messaging.Example.Business.Services
{
    /// <summary>
    /// Reads messages from a Kafka topic
    /// </summary>
    /// <typeparam name="TMessageType"></typeparam>
    public class ConsumerService<TMessageType>: IDisposable where TMessageType : MessageBase
    {
        private IConsumer<string, TMessageType> consumer;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="topic">The topic this consumer should subscribe to</param>
        public ConsumerService(string topic)
        {
            // set up consumer configuration
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            // create the consumer
            consumer = new ConsumerBuilder<string, TMessageType>(consumerConfig)
                .SetValueDeserializer(new MessageSerialiser<TMessageType>()) // because we are sending a custom object we need to tell Kafka how to deserialise it
                .Build();

            consumer.Subscribe(topic); // subscribe to the topic
        }

        /// <summary>
        /// Consumes a message from Kafka
        /// </summary>
        /// <returns></returns>
        public TMessageType ConsumeMessage()
        {
            try
            {
                var consumeResult = consumer.Consume();
                return consumeResult.Message.Value;
            }
            catch (ConsumeException ex)
            {
                // log the exception
                return default;
            }
        }

        public void Dispose()
        {
            consumer.Close();
            consumer.Dispose();
        }
    }
}
