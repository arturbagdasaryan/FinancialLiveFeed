using FinancialLiveFeed.Application.Interfaces;
using FinancialLiveFeed.Domain.Entities;
using System.Net.WebSockets;
using System.Text;

namespace FinancialLiveFeed.Infrastructure.Services
{
    public class PriceBroadcastService : IPriceBroadcastService
    {
        private readonly Dictionary<WebSocket, string> _subscribers = new();
        private readonly ILogger<PriceBroadcastService> _logger;

        public PriceBroadcastService(ILogger<PriceBroadcastService> logger)
        {
            _logger = logger;
        }

        public void AddSubscriber(string symbol, WebSocket socket)
        {
            _subscribers[socket] = symbol;
            _logger.LogInformation($"New subscriber for {symbol}.");
        }

        public void RemoveSubscriber(WebSocket socket)
        {
            _subscribers.Remove(socket);
            _logger.LogInformation("Subscriber disconnected.");
        }

        public async void BroadcastPrice(PriceUpdate priceUpdate)
        {
            foreach (var subscriber in _subscribers)
            {
                try
                {
                    // Check if WebSocket is still connected
                    if (subscriber.Key.State == WebSocketState.Open)
                    {
                        // Send message asynchronously
                        var message = $"Price update for {priceUpdate.Symbol}: {priceUpdate.Price}";
                        var buffer = Encoding.UTF8.GetBytes(message);
                        await subscriber.Key.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                        _logger.LogInformation($"Broadcasting price for {priceUpdate.Symbol}: {priceUpdate.Price}");
                    }
                    else
                    {
                        // Handle case where WebSocket is not open
                        _logger.LogWarning($"WebSocket for {priceUpdate.Symbol} is not open. Skipping broadcast.");
                    }
                }
                catch (Exception ex)
                {
                    // Log any exceptions that occur during broadcasting
                    _logger.LogError(ex, $"Error broadcasting price update for {priceUpdate.Symbol}");
                }
            }
        }

    }
}
