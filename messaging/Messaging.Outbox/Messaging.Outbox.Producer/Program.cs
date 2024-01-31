using Messaging.Outbox.Producer;

Console.Title = "Messaging.Example.Outbox.Producer";

Task.Run(async () => await new MessageSender().RenderMainMenu()).Wait();