using FinancialLiveFeed.Application.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace FinancialLiveFeed.Infrastructure.Services
{
    public class WebSocketHandler : IWebSocketHandler
    {
        private readonly IPriceBroadcastService _broadcastService;

        public WebSocketHandler(IPriceBroadcastService broadcastService)
        {
            _broadcastService = broadcastService;
        }

        public async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            // Simulate handling WebSocket data and subscription
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                // Assume client sends subscription request: {"method": "SUBSCRIBE", "params": ["BTCUSD"]}
                if (message.Contains("BTCUSD"))
                {
                    _broadcastService.AddSubscriber("BTCUSD", webSocket);
                }
            }
        }
    }
}
