using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Core.Entities;
using Valtuutus.RealWorld.Api.Results;

namespace Valtuutus.RealWorld.Api.Features.Users;

public record Login
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record LoginResponse
{
    public required string AccessToken { get; init; }
}

public class LoginHandler(Context context, TimeProvider timeProvider, IOptions<TokenOptions> tokenOptions) : IUseCase<Login, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(Login req, CancellationToken ct)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == req.Email.ToLower(), cancellationToken: ct);

        if (user == null)
        {
            return Result.Unauthorized();
        }

        var correctPassword = BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash);

        if (!correctPassword)
        {
            return Result.Unauthorized();
        }

        return new LoginResponse
        {
            AccessToken = GenerateToken(user.Id)
        };
    }
    
    private string GenerateToken(UserId userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value!.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var now = timeProvider.GetUtcNow().UtcDateTime;
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(now).ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiry = now.Add(tokenOptions.Value!.Expiration);

        var token = new JwtSecurityToken(
            issuer: "vaultuutus-real-world-example",
            audience: "localhost",
            claims: claims,
            expires: expiry,
            signingCredentials: credentials,
            notBefore: now
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}