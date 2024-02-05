using Messaging.Outbox.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Outbox.Producer";

Console.Title = "Outbox.Consumer1";
var host = new HostBuilder()
        .ConfigureHostConfiguration(configHost => {
        })
        .ConfigureServices((hostContext, services) => {
            services.AddHostedService<KafkaProducerService>();
        })
        .UseConsoleLifetime()
        .Build();

//run the host
host.Run();

//Task.Run(async () => await new MessageSender().RenderMainMenu()).Wait();