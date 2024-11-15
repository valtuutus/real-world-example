using System.Text.Json;
using System.Text.Json.Serialization;
using MassTransit;

namespace Valtuutus.RealWorld.Api.Consumers;

public class CdcConsumer : IConsumer<Batch<CdcEnvelope>>
{
    private readonly ILogger<CdcConsumer> _logger;

    public CdcConsumer(ILogger<CdcConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Batch<CdcEnvelope>> context)
    {
        _logger.LogInformation("CDC consumer received batch of {num}", context.Message.Length);
        foreach (var ctx  in context.Message)
        {
            _logger.LogInformation("CDC consumer received {@context}", ctx.Message);
        }
        return Task.CompletedTask;
    }
}

public record CdcEnvelope
{
    public CdcPayload Payload { get; set; }
}

public record CdcPayload
{
    public JsonDocument Before { get; set; }
    public JsonDocument After { get; set; }
    public CdcSource Source { get; set; }
    // TODO: this should probably be an enum
    [JsonPropertyName("op")]
    public string Operation { get; set; }
}

public record CdcSource
{
    public string Schema { get; set; }
    public string Table { get; set; }
}