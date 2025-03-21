namespace Valtuutus.RealWorld.Api.Core.Auth;

public class TokenOptions
{
    public required string Secret { get; init; }
    public TimeSpan Expiration { get; init; }
}