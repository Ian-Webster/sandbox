using Confluent.Kafka;
using Messaging.Example.Business.Models;
using Spectre.Console;

namespace Messaging.Example.Business.Services
{
    /// <summary>
    /// Sends messages to Kafka
    /// </summary>
    public class ProducerService
    {
        private IProducer<string, MessageBase> producer;

        public ProducerService()
        {
            // create configuation for the producer
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            // create the producer
            producer = new ProducerBuilder<string, MessageBase>(producerConfig)
                .SetValueSerializer(new MessageSerialiser<MessageBase>()) // because we are sending a custom object we need to tell Kafka how to serialise it
                .Build();
        }

        /// <summary>
        /// Sends a message to Kafka on the specified topic
        /// </summary>
        /// <param name="topic">topic to send the message to</param>
        /// <param name="message">message to send</param>
        public void Produce(string topic, MessageBase message)
        {
            producer.Produce(topic, new Message<string, MessageBase>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            },
            (deliveryReport) =>
            {
                // handle the result of sending the message
                if (deliveryReport.Error.Code != ErrorCode.NoError)
                    // failure
                    AnsiConsole.MarkupLine($"[bold red]Failed to send message {message} error was {deliveryReport.Error.Code}[/]");
                else
                    // success
                    AnsiConsole.MarkupLine($"[bold green]Successfully sent message {message}[/]");
            });
        }

        /// <summary>
        /// Flushes the producer
        /// </summary>
        public void Flush()
        {
            producer.Flush();
        }

    }
}
