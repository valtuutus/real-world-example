using System.Net.Mime;
using System.Text.Json;
using MassTransit;
using RabbitMQ.Client;
using Valtuutus.RealWorld.Api.Consumers;
using Vogen;

[assembly: VogenDefaults(
    underlyingType: typeof(Guid), 
    conversions: Conversions.Default, 
    throws: typeof(ValueObjectValidationException))]

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
            ec.ConfigureConsumer<CdcConsumer>(ctx);
        });
        
    });
    
});

builder.AddServiceDefaults();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.Run();

