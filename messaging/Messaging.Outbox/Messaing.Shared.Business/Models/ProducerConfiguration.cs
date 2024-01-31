namespace Messaing.Shared.Business.Models
{
    /// <summary>
    /// Holds configuration data for the producer
    /// </summary>
    /// <remarks>
    /// You should provide this configuration via appsettings.json or a secrets file / store
    /// </remarks>
    public class ProducerConfiguration
    {
        /// <summary>
        /// The Kafka host
        /// </summary>
        public string KafkaHost { get; set; }

        /// <summary>
        /// The name of the producer group
        /// </summary>
        public string ProducerGroupName { get; set; }

        /// <summary>
        /// Message timout in milliseconds
        /// </summary>
        public int MessageTimeout { get; set; } 

    }
}
