using Confluent.Kafka;

namespace BasicApi.Producers
{
    internal sealed class GreeterProducer
    {
        private const string TOPIC = "Greeter";

        private readonly IProducer<string, int> _producer;
        private int _count;

        public GreeterProducer(ProducerBuilder<string, int> producerBuilder)
        {
            _producer = producerBuilder.Build();
        }

        public void Produce( string value )
        {
            var message = new Message<string, int>() { Key = value, Value = _count };
            _producer.Produce(TOPIC, message);

            _count++;
        }
    }
}
