using Microsoft.EntityFrameworkCore;
using StronglyTypedIds;
using Valtuutus.RealWorld.Api.Config;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Features.Projects;
using Valtuutus.RealWorld.Api.Features.Teams;
using Valtuutus.RealWorld.Api.Features.Users;
using Valtuutus.RealWorld.Api.Features.Workspaces;

[assembly: StronglyTypedIdDefaults(Template.Guid, "guid-efcore")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<Context>(o =>
{
    o.UseNpgsql(connectionString: builder.Configuration.GetConnectionString("valtuutus"));
});

builder.AddServiceDefaults();

builder.Host.AddAuthSetup();
builder.Host.AddUseCases(typeof(Program).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.UseAuthentication();
app.UseMiddleware<SessionManagerMiddleware>();
app.UseAuthorization();

app.MapUsersEndpoints();;
app.MapWorkspaceEndpoints();
app.MapTeamsEndpoints();
app.MapTeamsEndpoints();
app.MapProjectsEndpoints();

app.Run();