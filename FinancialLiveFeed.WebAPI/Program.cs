using FinancialLiveFeed.Application.DTOs;
using FinancialLiveFeed.Application.Interfaces;
using FinancialLiveFeed.Infrastructure.Services;
using FinancialLiveFeed.Infrastructure.WebSocketClients;
using FinancialLiveFeed.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Shared cache for price lookup
var cache = new Dictionary<string, PriceDto>();

builder.Services.AddSingleton(cache);
builder.Services.AddSingleton<IInstrumentService, InstrumentService>();
builder.Services.AddSingleton<IPriceBroadcastService, PriceBroadcastService>();
builder.Services.AddSingleton<IPriceFeedClient, BinanceWebSocketClient>();
builder.Services.AddHostedService<PriceFeedWorker>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();
app.MapControllers();
app.Run();

// Background worker for price updates
public class PriceFeedWorker : BackgroundService
{
    private readonly IPriceFeedClient _client;
    public PriceFeedWorker(IPriceFeedClient client) => _client = client;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _client.StartAsync(stoppingToken);
    }
}
