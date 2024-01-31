using Confluent.Kafka;
using Messaing.Shared.Business.Models;

namespace Messaing.Shared.Business.Producer
{
    /// <summary>
    /// Provides a mechanism for sending messages to Kafka
    /// </summary>
    /// <typeparam name="TMessage">The type of message to send</typeparam>
    public interface IKafkaProducer<TMessage> where TMessage : MessageBase
    {
        /// <summary>
        /// Sends a message to Kafka
        /// </summary>
        /// <param name="topicName">name of the topic the message should be sent to</param>
        /// <param name="message">The message to send</param>
        /// <param name="token">cancellation token</param>
        /// <returns>
        /// Returns the message with some of the IMessage properties populated
        /// </returns>
        Task<DeliveryResult<string, TMessage>> SendMessage(string topicName, TMessage message, CancellationToken token);
    }
}
