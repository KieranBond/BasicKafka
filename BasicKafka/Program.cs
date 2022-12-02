using Confluent.Kafka;

var producerConfig = new ProducerConfig() 
{ 
    BootstrapServers = "localhost:9092"
};
var consumerConfig = new ConsumerConfig()
{
    BootstrapServers = producerConfig.BootstrapServers,
    GroupId = "BasicKafka",
    AutoOffsetReset = AutoOffsetReset.Earliest,
};

Console.WriteLine("Kafka topic: ");
var topic = Console.ReadLine();

var producerBuilder = new ProducerBuilder<Null, string>(producerConfig);
using var producer = producerBuilder.Build();

var consumerBuilder = new ConsumerBuilder<string, string>(consumerConfig);
using var consumer = consumerBuilder.Build();

consumer.Subscribe(topic);

for ( int i = 0; i < 5; i++ )
{
    Console.WriteLine("Input: ");
    var input = Console.ReadLine();
    if (input == null) continue;

    await producer.ProduceAsync(topic, new Message<Null, string> { Value = input });
}

Console.Clear();
Console.WriteLine($"Consuming for topic '{topic}'\n");

string? value;
do
{
    var output = consumer.Consume( TimeSpan.FromSeconds(5) );
    value = output?.Message?.Value;
    var key = output?.Message?.Key;
    var timestamp = output?.Message?.Timestamp;
    if( value != null ) Console.WriteLine($"Timestamp: {timestamp?.UtcDateTime ?? DateTime.UnixEpoch}\nKey: {key}, \nValue: {value}\n\n");
}
while ( value != null );