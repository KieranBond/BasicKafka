using Confluent.Kafka;
using gRPCContract;
using static Confluent.Kafka.ConfigPropertyNames;

namespace BasicApi.Consumers;

public sealed class GreeterConsumer : IDisposable
{
    private const string TOPIC = "Greeter";

    private readonly IConsumer<string, int> _consumer;

    private CancellationToken _cancelToken;

    public GreeterConsumer(ConsumerBuilder<string, int> consumerBuilder)
    {
        _consumer = consumerBuilder.Build();
        _consumer.Subscribe(TOPIC);

        _cancelToken = new CancellationToken(false);
    }

    public Task<GreeterResult> Consume()
    {
        var consumed = _consumer.Consume(_cancelToken);

        if (consumed == null || consumed.Message == null) return Task.FromResult<GreeterResult>( null );

        GreeterResult result = new()
        {
            Key = consumed.Message.Key,
            Value = consumed.Message.Value
        };
        return Task.FromResult(result);
    }

    public void Cancel()
    {
        _cancelToken = new CancellationToken(true);
    }

    public void Dispose()
    {
        Cancel();
        _consumer.Close();
    }
}