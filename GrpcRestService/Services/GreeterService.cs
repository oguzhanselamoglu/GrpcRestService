using Grpc.Core;
using GrpcRestService;

namespace GrpcRestService.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override async Task SendMessage(MessageRequest request, IServerStreamWriter<MessageReply> responseStream, ServerCallContext context)
    {
        Console.WriteLine(request.Text);
        Random random = new();
        await Task.Run(async () =>
        {
            int count = random.Next(3, 10);
            while (count-- > 0)
            {
                string text = (random.Next() * random.Next()).ToString();
                await responseStream.WriteAsync(new() { Text = text });
                Console.WriteLine($"{text} gönderilmiþtir.");
                await Task.Delay(1000);
            }
        });
    }
}

