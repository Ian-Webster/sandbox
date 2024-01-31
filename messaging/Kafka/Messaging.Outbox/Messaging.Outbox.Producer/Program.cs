using Messaging.Outbox.Producer;

Console.Title = "Outbox.Producer";

Task.Run(async () => await new MessageSender().RenderMainMenu()).Wait();