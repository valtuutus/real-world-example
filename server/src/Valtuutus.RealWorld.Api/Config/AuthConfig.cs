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
            services.AddScoped<IAuthorizationHandler, ViewWorkspaceHandler>();
            services.AddScoped<IAuthorizationHandler, AssignUserHandler>();
            services.AddScoped<IAuthorizationHandler, ViewProjectHandler>();
            services.AddScoped<IAuthorizationHandler, EditProjectHandler>();
            services.AddScoped<IAuthorizationHandler, CreateTaskHandler>();


            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = false;

                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                
                options.AddPolicy(AppPolicies.Workspace.View, policy => policy.
                    RequireAuthenticatedUser().AddRequirements(WorkspaceRequirements.View.Instance));
                
                options.AddPolicy(AppPolicies.Workspace.AssignUser, policy => policy.
                    RequireAuthenticatedUser().AddRequirements(WorkspaceRequirements.AssignUser.Instance));
                
                
                options.AddPolicy(AppPolicies.Workspace.CreateProject,
                    policy => policy.RequireAuthenticatedUser().AddRequirements(WorkspaceRequirements.CreateProject.Instance));
                
                
                options.AddPolicy(AppPolicies.Project.View, policy => policy.
                    RequireAuthenticatedUser().AddRequirements(ProjectRequirements.View.Instance));
                
                options.AddPolicy(AppPolicies.Project.Edit, policy => policy.
                    RequireAuthenticatedUser().AddRequirements(ProjectRequirements.Edit.Instance));
                
                
                options.AddPolicy(AppPolicies.Project.CreateTask, policy => policy.
                    RequireAuthenticatedUser().AddRequirements(ProjectRequirements.CreateTask.Instance));
            });
            
        });
    }
}