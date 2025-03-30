using FinancialLiveFeed.Application.Interfaces;
using FinancialLiveFeed.Infrastructure.Services;
using FinancialLiveFeed.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<IInstrumentService, InstrumentService>();
builder.Services.AddSingleton<IPriceBroadcastService, PriceBroadcastService>();
builder.Services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

// Build the application
var app = builder.Build();

// Enable WebSocket and Swagger
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();

// REST API endpoints
app.MapGet("/api/price/instruments", async (IInstrumentService instrumentService) =>
{
    var instruments = await instrumentService.GetAvailableInstrumentsAsync();
    return Results.Ok(instruments);
});

app.MapGet("/api/price/{symbol}", async (IInstrumentService instrumentService, string symbol) =>
{
    var price = await instrumentService.GetCurrentPriceAsync(symbol);
    return Results.Ok(price);
});

// Swagger UI for testing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
