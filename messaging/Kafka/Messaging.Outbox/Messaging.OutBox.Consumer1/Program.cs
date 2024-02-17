using Confluent.Kafka;
using Messaging.OutBox.Consumer1;
using Messaing.Shared.Business.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Outbox.Consumer1";
var host = new HostBuilder()
        .ConfigureHostConfiguration(configHost => {
        })
        .ConfigureServices((hostContext, services) => {
            services.AddHostedService<KafkaConsumerService>();
            services.AddHostedService<ClockTickService>();
            services.Configure<ConsumerConfiguration>(options =>
            {
                options.KafkaHost = "localhost:9092";
                options.ConsumerGroupName = "OutboxConsumer1";
                options.AutoOffsetReset = AutoOffsetReset.Earliest;
                options.SessionTimeout = 6000;
            });
        })
        .UseConsoleLifetime()
        .Build();

//run the host
host.Run();
Console.ReadKey();