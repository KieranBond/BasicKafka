using BasicApi.Producers;
using Grpc.Core;
using gRPCContract;

namespace gRPCContract.Services
{
    internal class GreeterService : Greeter.GreeterBase
    {
        private readonly Random _random;
        private readonly ILogger<GreeterService> _logger;
        private readonly GreeterProducer _producer;

        public GreeterService(ILogger<GreeterService> logger, GreeterProducer producer)
        {
            _random = new Random();
            _logger = logger;
            _producer = producer;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            bool success = _random.Next(1) == 0;
            _logger.LogDebug("Rolled {success}", success);

            if( !success )
            {
                var metaData = new Metadata()
                {
                    { "Request name", request.Name },
                };
                throw new RpcException(new Status(StatusCode.Internal, "Internal roll of dice"), metaData );
            }

            _producer.Produce(request.Name);

            return Task.FromResult(new HelloReply
            {
                Message = $"Hello {request.Name}"
            });
        }
    }
}