using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using FinancialLiveFeed.Application.DTOs;
using FinancialLiveFeed.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialLiveFeed.Infrastructure.WebSocketClients;

public class PriceBroadcastService : IPriceBroadcastService
{
    private readonly Dictionary<string, List<WebSocket>> _subscribers = new();
    private readonly ILogger<PriceBroadcastService> _logger;
    private readonly object _lock = new();

    public PriceBroadcastService(ILogger<PriceBroadcastService> logger) => _logger = logger;

    public void AddSubscriber(string symbol, WebSocket socket)
    {
        lock (_lock)
        {
            symbol = symbol.ToUpper();
            if (!_subscribers.ContainsKey(symbol))
                _subscribers[symbol] = new();
            _subscribers[symbol].Add(socket);
            _logger.LogInformation("New subscriber: {symbol}", symbol);
        }
    }

    public void RemoveSubscriber(WebSocket socket)
    {
        lock (_lock)
        {
            foreach (var s in _subscribers.Values)
                s.Remove(socket);
            _logger.LogInformation("Subscriber removed");
        }
    }

    public void BroadcastPrice(PriceDto price)
    {
        var data = JsonSerializer.Serialize(price);
        var buffer = Encoding.UTF8.GetBytes(data);
        if (_subscribers.TryGetValue(price.Symbol.ToUpper(), out var sockets))
        {
            foreach (var s in sockets.ToList())
            {
                if (s.State == WebSocketState.Open)
                    s.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
