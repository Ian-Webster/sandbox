using Messaging.Example.Business.Models;
using Messaging.Example.Business.Services;
using Spectre.Console;

namespace Messaging.Example.Consumer1
{
    public class MessageConsumer
    {
        private ConsumerService<HelloAllMessage> _allConsumer;
        private ConsumerService<HelloConsumer1Message> _consumer1;

        public MessageConsumer()
        {
            _allConsumer = new ConsumerService<HelloAllMessage>("HelloAll");
            _consumer1 = new ConsumerService<HelloConsumer1Message>("HelloConsumer1");
        }

        public void StartConsuming()
        { 
            AnsiConsole.MarkupLine($"[bold magenta3]Consumer1 listening for messages[/]");
            //ConsumerAllMessages();
            var thread1 = new System.Threading.Thread(() => ConsumerAllMessages()) { IsBackground = true};
            var thread2 = new System.Threading.Thread(() => Consumer1Messages()) { IsBackground = true };
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

        public void Consumer1Messages()
        {
            int counter = 1;
            while (true)
            {
                var message = _consumer1.ConsumeMessage();
                AnsiConsole.MarkupLine($"[bold magenta3]Successfully received message {counter} {message}[/]");
                counter++;
            }
        }

    }
}
