namespace Valtuutus.RealWorld.Api.Core.Auth;

public class TokenOptions
{
    public string Secret { get; set; } = "SECRET-MUITO-SEGURO-LEGAL-🔒🔒🔒🔒🔒🔒";
    public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(1);
}