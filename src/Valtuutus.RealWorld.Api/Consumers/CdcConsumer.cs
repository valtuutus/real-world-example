using System.Text.Json;
using MassTransit;

namespace Valtuutus.RealWorld.Api.Consumers;

public class CdcConsumer : IConsumer<Message>
{
    public Task Consume(ConsumeContext<Message> context)
    {
        throw new NotImplementedException();
    }
}

public record Message
{
    public JsonDocument Payload { get; init; }
    
}