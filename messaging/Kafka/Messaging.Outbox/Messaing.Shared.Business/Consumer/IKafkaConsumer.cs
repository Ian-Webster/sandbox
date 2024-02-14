using Confluent.Kafka;
using Messaing.Shared.Business.Models;

namespace Messaing.Shared.Business.Consumer
{
    /// <summary>
    /// Provides a mechanism for consuming messages from Kafka
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IKafkaConsumer<TMessage> where TMessage : MessageBase
    {
        /// <summary>
        /// Indicates whether a consumer is subscribed to a topic/s or not
        /// </summary>
        public bool Subscribed { get; set; }

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
        ConsumeResult<string, TMessage>? ConsumeMessage(CancellationToken token);

        /// <summary>
        /// Commits a message to Kafka
        /// </summary>
        /// <param name="message"></param>
        bool CommitMessage(ConsumeResult<string, TMessage> message);
    }
}
