using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using FinancialLiveFeed.Application.DTOs;
using FinancialLiveFeed.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialLiveFeed.Infrastructure.WebSocketClients;

public class BinanceWebSocketClient : IPriceFeedClient
{
    private readonly Uri _uri = new("wss://stream.binance.com:443/ws/btcusdt@aggTrade");
    private readonly Dictionary<string, PriceDto> _cache;
    private readonly IPriceBroadcastService _broadcaster;
    private readonly ILogger<BinanceWebSocketClient> _logger;

    public BinanceWebSocketClient(Dictionary<string, PriceDto> cache, IPriceBroadcastService broadcaster, ILogger<BinanceWebSocketClient> logger)
    {
        _cache = cache;
        _broadcaster = broadcaster;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        try
        {
            using var ws = new ClientWebSocket();
            await ws.ConnectAsync(_uri, ct);
            _logger.LogInformation("Connected to Binance WS");

            var buffer = new byte[4096];
            while (!ct.IsCancellationRequested)
            {
                var result = await ws.ReceiveAsync(buffer, ct);
                if (result.MessageType == WebSocketMessageType.Close) break;

                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("p", out var priceProp) &&
                    doc.RootElement.TryGetProperty("T", out var tsProp))
                {
                    var price = decimal.Parse(priceProp.GetString()!);
                    var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(tsProp.GetInt64()).UtcDateTime;
                    var dto = new PriceDto { Symbol = "BTCUSD", Price = price, Timestamp = timestamp };
                    _cache["BTCUSD"] = dto;
                    _broadcaster.BroadcastPrice(dto);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Binance connection failed");
        }
    }
}
