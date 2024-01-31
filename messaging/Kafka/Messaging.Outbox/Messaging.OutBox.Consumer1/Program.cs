using Messaging.OutBox.Consumer1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Outbox.Consumer1";
var host = new HostBuilder()
        .ConfigureHostConfiguration(configHost => {
        })
        .ConfigureServices((hostContext, services) => {
            services.AddHostedService<KafkaConsumerService>();
        })
        .UseConsoleLifetime()
        .Build();

//run the host
host.Run();
Console.ReadKey();

