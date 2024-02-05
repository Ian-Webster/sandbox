using DataAccess.Repository;
using Messaging.Outbox.Data;
using Messaging.Outbox.Data.Repositories;
using Messaging.Outbox.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Outbox.Producer";

var host = new HostBuilder()
        .ConfigureHostConfiguration(configHost => {
        })
        .ConfigureServices((hostContext, services) => {
            services.AddScoped<RepositoryFactory<OutboxContent>>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<MessageSender>();
            services.AddHostedService<KafkaProducerService>();
            services.AddDbContext<OutboxContent>();
        })
        .UseConsoleLifetime()
        .Build();

var messageSender = host.Services.GetRequiredService<MessageSender>();

Task.Run(async () => await messageSender.RenderMainMenu());

//run the host
host.Run();
