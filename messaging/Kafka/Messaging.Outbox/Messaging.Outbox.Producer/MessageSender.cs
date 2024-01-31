using Confluent.Kafka;
using Messaging.Outbox.Domain.Messages;
using Messaing.Shared.Business.Models;
using Messaing.Shared.Business.Producer;
using Spectre.Console;

namespace Messaging.Outbox.Producer
{
    public class MessageSender
    {
        private readonly IKafkaProducer<OutboxMessageBase> producer;

        public MessageSender()
        {
            producer = new KafkaProducer<OutboxMessageBase>(
                new ProducerConfiguration
                {
                    KafkaHost = "localhost:9092",
                    ProducerGroupName = "OutboxProducer",
                    MessageTimeout = 10000
                }
            );
        }

        public async Task RenderMainMenu()
        {
            Console.Clear();

            var messageType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Which type of message would you like to send?")
                    .PageSize(10)
                    .AddChoices(new[] {
            "Hello All Consumers", "Hello Consumer 1", "Hello Consumer 2", "Random", "Exit"
                    }));

            switch (messageType)
            {
                case "Hello All Consumers":
                    await RenderHowManyMenu(MessageTypes.HelloAll);
                    break;
                case "Hello Consumer 1":
                    await RenderHowManyMenu(MessageTypes.HelloConsumer1);
                    break;
                case "Hello Consumer 2":
                    await RenderHowManyMenu(MessageTypes.HelloConsumer2);
                    break;
                case "Random":
                    await RenderHowManyMenu(MessageTypes.Random);
                    break;
                case "Exit":
                    return;
                default:
                    AnsiConsole.MarkupLine($"[bold red]Invalid message type[/][/]");
                    break;
            }
        }

        private async Task RenderHowManyMenu(MessageTypes messageType)
        {
            var numberToSend = AnsiConsole.Prompt(
                    new TextPrompt<int>($"How many {messageType} messages would you like to send? If you want a random amount enter 0")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid number[/]")
                        .Validate(number =>
                        {
                            return number switch
                            {
                                < 0 => ValidationResult.Error("[red]You must enter zero or greater[/]"),
                                _ => ValidationResult.Success(),
                            };
                        }));

            if (numberToSend == 0) numberToSend = new System.Random().Next(1, 100);

            Console.WriteLine();

            if (await SendMessages(messageType, numberToSend))
                AnsiConsole.MarkupLine($"[bold green]Sent {numberToSend} {messageType} messages[/]");
            else
                AnsiConsole.MarkupLine($"[bold red]Failed to send {numberToSend} {messageType} messages[/]");

            Console.WriteLine();

            AnsiConsole.MarkupLine($"If you want to send more messages press space otherwise press any other key to exit");

            var key = System.Console.ReadKey();

            if (key.Key == System.ConsoleKey.Spacebar)
                await RenderMainMenu();
        }

        private async Task<bool> SendMessages(MessageTypes messageType, int numberToSend)
        {
            for (int i = 0; i < numberToSend; i++)
            {
                var messageData = CreateMessage(messageType);

                AnsiConsole.MarkupLine($"[bold green]Generated message {messageData.message} {i + 1} of {numberToSend}[/]");

                var result = await producer.SendMessage(messageData.topicName, messageData.message, new System.Threading.CancellationToken());
                if (result.Status != PersistenceStatus.Persisted)
                {
                    // Handle failed message send
                    AnsiConsole.MarkupLine($"[bold red]Failed to send message {i + 1}[/]");
                    return false;
                }
            }
            return true;
        }

        private (OutboxMessageBase message, string topicName) CreateMessage(MessageTypes messageType)
        {
            switch (messageType)
            {
                case MessageTypes.HelloAll:
                    return (new HelloAll(), "Outbox-HelloAll");
                case MessageTypes.HelloConsumer1:
                    return (new HelloConsumer1(), "Outbox-HelloConsumer1");
                case MessageTypes.HelloConsumer2:
                    return (new HelloConsumer2(), "Outbox-HelloConsumer2");
                case MessageTypes.Random:
                    {
                        var bogus = new Bogus.Faker();
                        return CreateMessage(bogus.PickRandomWithout(MessageTypes.Random));
                    }
                default:
                    throw new Exception("Invalid message type");
            }
        }

    }
}
