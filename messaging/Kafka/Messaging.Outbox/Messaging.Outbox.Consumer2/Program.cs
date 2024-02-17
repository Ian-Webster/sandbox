using Confluent.Kafka;
using Messaging.Outbox.Consumer2;
using Messaging.Outbox.Consumer2.Services;
using Messaging.Outbox.Domain.Messages;
using Messaging.Shared.Business.Consumer;
using Messaing.Shared.Business.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IKafkaConsumer<OutboxMessageBase>, KafkaConsumer<OutboxMessageBase>>();
builder.Services.Configure<ConsumerConfiguration>(options =>
{
    options.KafkaHost = "localhost:9092";
    options.ConsumerGroupName = "OutboxConsumer2";
    options.AutoOffsetReset = AutoOffsetReset.Earliest;
    options.SessionTimeout = 6000;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();