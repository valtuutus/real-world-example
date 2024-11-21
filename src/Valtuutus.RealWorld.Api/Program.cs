using System.Net.Mime;
using MassTransit;
using Valtuutus.RealWorld.Api.Consumers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StronglyTypedIds;
using Valtuutus.RealWorld.Api.Core;
using Valtuutus.RealWorld.Api.Features.Workspaces.Create;
using Valtuutus.RealWorld.Api.Results;

[assembly:StronglyTypedIdDefaults(Template.Guid, "guid-efcore")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMassTransit(e =>
{
    e.AddConsumer<CdcConsumer>();
    e.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq://guest:guest@localhost:5672");
        cfg.ReceiveEndpoint("valtuutus-cdc", ec =>
        {
            ec.ConfigureConsumeTopology = false;
            ec.DefaultContentType = new ContentType("application/json");
            ec.UseRawJsonSerializer();
            ec.Bind("valtuutus-cdc");
            ec.PrefetchCount = 10;
            ec.ConfigureConsumer<CdcConsumer>(ctx, a =>
            {
                a.ConcurrentMessageLimit = 10;
                a.Options<BatchOptions>(opts =>
                {
                    opts
                        .SetTimeLimit(TimeSpan.FromMilliseconds(5))
                        .SetTimeLimitStart(BatchTimeLimitStart.FromLast)
                        .SetConcurrencyLimit(3);
                });
            });
        });
        
    });
    
});

builder.Services.AddDbContext<Context>(o =>
{
    o.UseNpgsql(connectionString: builder.Configuration.GetConnectionString("valtuutus"));
});

builder.AddServiceDefaults();

builder.Services.AddMediatR(cfg =>
{
    cfg.Lifetime = ServiceLifetime.Scoped;
    cfg.RegisterServicesFromAssemblyContaining<CreateWorkspaceHandler>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();


app.MapPost("workspace", async (IMediator mediator, [FromBody] CreateWorkspaceRequest request, CancellationToken ct) =>
{
    var result = await mediator.Send(request, ct);
    return result.ToApiResult();
});

app.Run();

