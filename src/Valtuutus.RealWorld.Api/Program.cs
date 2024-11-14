using Vogen;

[assembly: VogenDefaults(
    underlyingType: typeof(Guid), 
    conversions: Conversions.Default, 
    throws: typeof(ValueObjectValidationException))]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddServiceDefaults();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
