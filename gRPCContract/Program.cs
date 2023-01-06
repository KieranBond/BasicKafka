using BasicApi.Consumers;
using BasicApi.Producers;
using Confluent.Kafka;
using gRPCContract.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc( options =>
{
    options.EnableDetailedErrors = true;
});

var producerConfig = new ProducerConfig()
{
    BootstrapServers = "localhost:9092"
};

var consumerConfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    GroupId = "BasicKafka",
    AutoOffsetReset = AutoOffsetReset.Earliest,
};

builder.Services.AddSingleton(new ProducerBuilder<string, int>(producerConfig));
builder.Services.AddSingleton(new ConsumerBuilder<string, int>(consumerConfig));
builder.Services.AddSingleton<GreeterProducer>();
builder.Services.AddSingleton<GreeterConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
