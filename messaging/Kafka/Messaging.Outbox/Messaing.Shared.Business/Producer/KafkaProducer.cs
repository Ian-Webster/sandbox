using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Messaing.Shared.Business.Models;
using Messaing.Shared.Business.Serialisers;

namespace Messaging.Shared.Business.Producer
{
    public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>, IDisposable where TMessage : MessageBase
    {
        public bool IsFaulted { get; set; }

        private readonly IProducer<string, TMessage> _producer;
        private readonly IAdminClient _adminClient;

        public KafkaProducer(ProducerConfiguration config)
        {
            // create configuration for the producer
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config.KafkaHost,
                ClientId = config.ProducerGroupName,
                EnableIdempotence = true,
            };
            // create the producer
            _producer = new ProducerBuilder<string, TMessage>(producerConfig)
                .SetValueSerializer(new ByteArraySerialiser<TMessage>()) // because we are sending a custom object we need to tell Kafka how to serialise it
                .SetErrorHandler(((producer, error) =>
                {
                    IsFaulted = true;
                }))
                .Build();

            // create the admin client
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = config.KafkaHost
            };
            _adminClient = new AdminClientBuilder(adminConfig).Build(); // we'll use the admin client to check the status of the cluster
        }

        public void TEst()
        {
            var thing = new AdminClientBuilder(new AdminClientConfig()
            {
                BootstrapServers = "localhost:9092"
            });
            var thing2 = thing.Build();
            thing2.DescribeClusterAsync();
        }

        public async Task<DeliveryResult<string, TMessage>?> SendMessage(string topicName, TMessage message, CancellationToken token)
        {
            try
            {
                var result = await _producer.ProduceAsync(topicName,
                    new Message<string, TMessage> { Key = message.MessageId.ToString(), Value = message }, token);

                if (result.Status == PersistenceStatus.Persisted)
                {
                    IsFaulted = false;
                }

                return result;
            }
            catch (ProduceException<string, TMessage> ex)
            {
                return ex.DeliveryResult;
            }
            catch (Exception ex)
            {
                // log the exception
                return null;
            }
        }

        public async Task<bool> KafkaIsUp()
        {
            try
            {
                var test = await _adminClient.DescribeClusterAsync(new DescribeClusterOptions
                {
                    RequestTimeout = TimeSpan.FromSeconds(5) // we'll wait 5 seconds for the cluster to respond
                });
                IsFaulted = false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _producer?.Flush(TimeSpan.FromSeconds(10));
            _producer?.Dispose();
        }
    }
}
