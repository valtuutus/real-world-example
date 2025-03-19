using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Valtuutus.Lang;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Policies;

namespace Valtuutus.RealWorld.Api.Config;

public static class AuthConfig
{
      public static IHostBuilder AddAuthSetup(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            var jwtSection = context.Configuration.GetSection("Jwt");
            var jwtOptions = jwtSection.Get<TokenOptions>();
            services.Configure<TokenOptions>(jwtSection);
            if (jwtOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtOptions));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
            
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
                    }
                );
            services.AddScoped<IAuthorizationHandler, CreateProjectHandler>();
            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = false;
                options.AddPolicy(SchemaConstsGen.Workspace.Permissions.CreateProject, policy => policy.
                    AddRequirements(WorkspaceRequirements.CreateProject.Instance));
            });
            
        });
    }
}