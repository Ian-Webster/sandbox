using Confluent.Kafka;
using Messaging.Example.Business.Models;
using Messaging.Example.Business.Services;
using Spectre.Console;

namespace Messaging.Example.Consumer1
{
    public class MessageConsumer
    {
        private ConsumerService<HelloAllMessage> _allConsumer;

        public MessageConsumer()
        {
            _allConsumer = new ConsumerService<HelloAllMessage>("HelloAll");
        }

        public void StartConsuming()
        {
            int counter = 1;
            AnsiConsole.MarkupLine($"[bold teal]Consumer1 listening for messages[/]");
            while (true)
            {
                var message = _allConsumer.ConsumeMessage();
                AnsiConsole.MarkupLine($"[bold teal]Successfully received message {counter} {message}[/]");
                counter++;
            }
        }

    }
}
