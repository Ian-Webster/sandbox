using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Messaing.Shared.Business.Models;
using Messaing.Shared.Business.Serialisers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messaging.Shared.Business.Consumer
{
    public class KafkaConsumer<TMessage> : IKafkaConsumer<TMessage>, IDisposable where TMessage : MessageBase
    {
        private readonly ConsumerConfiguration _config;
        private readonly ILogger<IKafkaConsumer<TMessage>> _logger;
        private IConsumer<string, TMessage> _consumer;
        private readonly IAdminClient _adminClient;

        public KafkaConsumer(IOptions<ConsumerConfiguration> consumerConfigurationOptions, ILogger<IKafkaConsumer<TMessage>> logger)
        {
            _config = consumerConfigurationOptions.Value;
            _logger = logger;
            _consumer = new ConsumerBuilder<string, TMessage>(
                new ConsumerConfig
                {
                    BootstrapServers = _config.KafkaHost,
                    GroupId = _config.ConsumerGroupName,
                    AutoOffsetReset = _config.AutoOffsetReset,
                    EnableAutoCommit = false,
                    EnablePartitionEof = true,
                    SessionTimeoutMs = _config.SessionTimeout
                })
                .SetValueDeserializer(new ByteArraySerialiser<TMessage>())
                .SetErrorHandler((_, _) => { IsFaulted = true; })
                .Build();

            // create the admin client
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = _config.KafkaHost
            };
            _adminClient = new AdminClientBuilder(adminConfig).Build(); // we'll use the admin client to check the status of the cluster
        }

        public bool IsFaulted { get; set; }
        public bool IsSubscribed { get; set; }

        public bool SubscribeToTopic(string topicName)
        {
            try
            {
                _consumer.Subscribe(topicName);
                IsSubscribed = true;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to subscribe to topic {topicName}", topicName);
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
                _logger.LogError(ex, "Failed to subscribe to topics {topicNames}", string.Join(",", topicNames));
                return false;
            }
        }

        public ConsumeResult<string, TMessage>? ConsumeMessage()
        {
            try
            {
                return _consumer.Consume(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to consume message from Kafka");
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
                _logger.LogError(ex, "Failed to commit message to Kafka");
                return false;
            }
        }

        public async Task<bool> KafkaIsUp()
        {
            try
            {
                await _adminClient.DescribeClusterAsync(new DescribeClusterOptions
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
            _consumer.Close();
            _consumer.Dispose();
        }

        
    }
}
