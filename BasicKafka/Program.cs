using Confluent.Kafka;
using System.ComponentModel;

var consumerConfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    GroupId = "BasicKafka",
    AutoOffsetReset = AutoOffsetReset.Earliest,
};

Console.WriteLine("Kafka topic: ");
var topic = Console.ReadLine();

var consumerBuilder = new ConsumerBuilder<string, int>(consumerConfig);
using var consumer = consumerBuilder.Build();

consumer.Subscribe(topic);

Console.WriteLine($"Consuming for topic '{topic}'\n");

var cancelTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cancelTokenSource.Cancel();
};

int? value;
try
{
    do
    {
        var result = consumer.Consume( cancelTokenSource.Token );
        value = result?.Message?.Value;
        var key = result?.Message?.Key;
        var timestamp = result?.Message?.Timestamp;
        if( value != null ) Console.WriteLine($"Timestamp: {timestamp?.UtcDateTime ?? DateTime.UnixEpoch}\nKey: {key}, \nValue: {value}\n\n");
    }
    while ( true );
}
catch { }
finally
{
    consumer.Close();
}