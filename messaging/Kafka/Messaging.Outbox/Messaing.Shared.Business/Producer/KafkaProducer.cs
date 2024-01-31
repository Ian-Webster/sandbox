using Confluent.Kafka;
using Messaing.Shared.Business.Models;
using Messaing.Shared.Business.Serialisers;

namespace Messaing.Shared.Business.Producer
{
    public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>, IDisposable where TMessage : MessageBase
    {
        private IProducer<string, TMessage> _producer;

        public KafkaProducer(ProducerConfiguration config)
        {
            // create configuation for the producer
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config.KafkaHost,
                ClientId = config.ProducerGroupName,
                EnableIdempotence = true,
            };
            // create the producer
            _producer = new ProducerBuilder<string, TMessage>(producerConfig)
                .SetValueSerializer(new ByteArraySerialiser<TMessage>()) // because we are sending a custom object we need to tell Kafka how to serialise it
                .Build();
        }

        public void Dispose()
        {
            _producer?.Flush(TimeSpan.FromSeconds(10));
            _producer?.Dispose();
        }

        public async Task<DeliveryResult<string, TMessage>> SendMessage(string topicName, TMessage message, CancellationToken token)
        {
            try
            {
                 return await _producer.ProduceAsync(topicName, new Message<string, TMessage> { Key = message.MessageId.ToString(), Value = message }, token);
            }
            catch (ProduceException<string, TMessage> ex)
            {
                return ex.DeliveryResult;
            }
        }
    }
}
