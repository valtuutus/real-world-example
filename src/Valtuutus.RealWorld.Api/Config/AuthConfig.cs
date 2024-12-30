using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Valtuutus.RealWorld.Api.Core.Auth;

namespace Valtuutus.RealWorld.Api.Config;

public static class AuthConfig
{
      public static IHostBuilder AddAuthSetup(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            var base64Key = context.Configuration.GetValue<string>("JwtSigningKey");
            var key = base64Key == null ? null : new SymmetricSecurityKey(WebEncoders.Base64UrlDecode(base64Key));
            
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                    // Opções de validação
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }
                );
            
            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = false;

                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            
        });
    }
}