using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Scalar.AspNetCore;
using StronglyTypedIds;
using Valtuutus.Core.Configuration;
using Valtuutus.Data;
using Valtuutus.Data.Postgres;
using Valtuutus.RealWorld.Api.Config;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Core.Auth;
using Valtuutus.RealWorld.Api.Features.Projects;
using Valtuutus.RealWorld.Api.Features.Tasks;
using Valtuutus.RealWorld.Api.Features.Teams;
using Valtuutus.RealWorld.Api.Features.Users;
using Valtuutus.RealWorld.Api.Features.Workspaces;

[assembly: StronglyTypedIdDefaults(Template.Guid, "guid-efcore")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("valtuutus");

builder.Services.AddDbContext<Context>(o =>
{
    o.UseNpgsql(connectionString);
});

var schemaFilePath = Assembly.GetExecutingAssembly()
    .GetManifestResourceNames()
    .First(c => c.EndsWith("schema.vtt"));
var schema = Assembly.GetExecutingAssembly().GetManifestResourceStream(schemaFilePath)!;

builder.Services.AddValtuutusCore(schema)
    .AddPostgres(_ => () => new NpgsqlConnection(connectionString));

builder.AddServiceDefaults();
builder.Services.AddCors();

builder.Services.AddScoped<SessionManagerMiddleware>();
builder.Services.AddScoped<ISessionManager, SessionManager>();
builder.Host.AddAuthSetup();
builder.Host.AddUseCases(typeof(Program).Assembly);


var app = builder.Build();
app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.UseHttpsRedirection();
app.MapDefaultEndpoints();
app.UseAuthentication();
app.UseMiddleware<SessionManagerMiddleware>();
app.UseAuthorization();

app.MapUsersEndpoints();;
app.MapWorkspaceEndpoints();
app.MapTeamsEndpoints();
app.MapProjectsEndpoints();
app.MapTaskEndpoints();

app.Run();