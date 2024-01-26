using Messaging.Example.Business.Models;
using Messaging.Example.Business.Services;
using Spectre.Console;

namespace Messaging.Example.Consumer2
{
    public class MessageConsumer
    {
        private ConsumerService<HelloAllMessage> _allConsumer;
        private ConsumerService<HelloConsumer2Message> _consumer2;

        public MessageConsumer()
        {
            _allConsumer = new ConsumerService<HelloAllMessage>("HelloAll", "Consumer2");
            _consumer2 = new ConsumerService<HelloConsumer2Message>("HelloConsumer2", "Consumer2");
        }

        public void StartConsuming()
        { 
            AnsiConsole.MarkupLine($"[bold orange4_1]Consumer2 listening for messages[/]");
            var thread1 = new Thread(() => ConsumerAllMessages()) { IsBackground = true };
            var thread2 = new Thread(() => Consumer2Messages()) { IsBackground = true };
            thread1.Start();
            thread2.Start();
            while(true)
            {

            }
        }

        public void ConsumerAllMessages()
        {
            int counter = 1;
            while (true)
            {
                var message = _allConsumer.ConsumeMessage();
                AnsiConsole.MarkupLine($"[bold teal]Successfully received message {counter} {message}[/]");
                counter++;
            }
        }

        public void Consumer2Messages()
        {
            int counter = 1;
            while (true)
            {
                var message = _consumer2.ConsumeMessage();
                AnsiConsole.MarkupLine($"[bold orange4_1]Successfully received message {counter} {message}[/]");
                counter++;
            }
        }

    }
}
