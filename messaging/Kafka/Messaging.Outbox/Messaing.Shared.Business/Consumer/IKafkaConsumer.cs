using Confluent.Kafka;
using Messaing.Shared.Business.Models;

namespace Messaging.Shared.Business.Consumer
{
    /// <summary>
    /// Provides a mechanism for consuming messages from Kafka
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IKafkaConsumer<TMessage> where TMessage : MessageBase
    {
        /// <summary>
        /// Indicates whether the producer is faulted or not
        /// </summary>
        /// <remarks>
        /// This will be set automatically internally by the KafkaSubscriber class
        /// In the event of an error, this will be true
        /// </remarks>
        public bool IsFaulted { get; }

        /// <summary>
        /// Indicates whether the consumer is subscribed to a topic or not
        /// </summary>
        public bool IsSubscribed { get; }

        /// <summary>
        /// subscribes to a topic
        /// </summary>
        /// <remarks>
        /// You'll need to call this or <see cref="SubscribeToTopics"/> before calling <see cref="ConsumeMessage"/> 
        /// </remarks>
        /// <param name="topicName"></param>
        bool SubscribeToTopic(string topicName);

        /// <summary>
        /// subscribes to a collection of topics
        /// </summary>
        /// <remarks>
        /// You'll need to call this or <see cref="SubscribeToTopic"/> before calling <see cref="ConsumeMessage"/>
        /// </remarks>
        /// <param name="topicNames"></param>
        bool SubscribeToTopics(ICollection<string> topicNames);

        /// <summary>
        /// Consumes a message from Kafka
        /// </summary>
        /// <returns></returns>
        ConsumeResult<string, TMessage>? ConsumeMessage();

        /// <summary>
        /// Commits a message to Kafka
        /// </summary>
        /// <param name="message"></param>
        bool CommitMessage(ConsumeResult<string, TMessage> message);

        /// <summary>
        /// Checks if the Kafka cluster is up
        /// </summary>
        /// <remarks>
        /// Also sets "IsFaulted" to false if the cluster is up
        /// </remarks>
        /// <returns></returns>
        Task<bool> KafkaIsUp();
    }
}
