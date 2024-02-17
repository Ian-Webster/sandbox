using System.Text;
using Messaging.Outbox.Data.Entities;
using Messaging.Outbox.Data.Enums;
using Messaging.Outbox.Data.Repositories;
using Messaging.Outbox.Domain.Messages;
using Newtonsoft.Json;
using Spectre.Console;
using System.Text.Json;

namespace Messaging.Outbox.Producer
{
    public class MessageSender
    {
        private readonly IMessageRepository _messageRepo;

        public MessageSender(IMessageRepository messageRepository)
        {
            _messageRepo = messageRepository;
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

            if (numberToSend == 0) numberToSend = new Random().Next(1, 100);

            Console.WriteLine();

            if (await SendMessages(messageType, numberToSend))
                AnsiConsole.MarkupLine($"[bold green]Sent {numberToSend} {messageType} messages[/]");
            else
                AnsiConsole.MarkupLine($"[bold red]Failed to send {numberToSend} {messageType} messages[/]");

            Console.WriteLine();

            AnsiConsole.MarkupLine($"If you want to send more messages press space otherwise press any other key to exit");

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.Spacebar)
                await RenderMainMenu();
        }

        private async Task<bool> SendMessages(MessageTypes messageType, int numberToSend)
        {
            for (var i = 0; i < numberToSend; i++)
            {
                var messageData = CreateMessage(messageType);

                if (await _messageRepo.AddMessage(new Message
                    {
                        MessageId = Guid.NewGuid(),
                        Topic = messageData.topicName,
                        // using Netwonsoft.Json because the built in serialiser doesn't support serialising the base class
                        MessageContent = Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(messageData.message, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.Auto
                            })
                        ),
                        CreatedDate = DateTime.UtcNow,
                        Status = MessageStatus.Saved
                    }, new CancellationToken()))
                {
                    AnsiConsole.MarkupLine($"[bold green]Generated message {messageData.message} {i + 1} of {numberToSend}[/]");
                    continue;
                }

                AnsiConsole.MarkupLine($"[bold red]Failed to generate message {i + 1}[/]");
                return false;
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
