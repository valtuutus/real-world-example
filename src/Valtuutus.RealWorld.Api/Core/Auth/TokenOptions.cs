namespace Valtuutus.RealWorld.Api.Core.Auth;

public class TokenOptions
{
    public string Secret { get; init; } = "SECRET-MUITO-SEGURO-LEGAL-🔒🔒🔒🔒🔒🔒";
    public TimeSpan Expiration { get; init; } = TimeSpan.FromDays(1);
}