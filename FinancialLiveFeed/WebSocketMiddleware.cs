using FinancialLiveFeed.Application.Interfaces;
using System.Net.WebSockets;
using System.Text.Json;

namespace FinancialLiveFeed.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, IPriceBroadcastService broadcaster)
        {
            if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
            {
                using var socket = await context.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[1024 * 4];

                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var json = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var doc = JsonDocument.Parse(json);
                        var symbol = doc.RootElement.GetProperty("symbol").GetString();
                        if (doc.RootElement.GetProperty("type").GetString() == "subscribe" && symbol is not null)
                        {
                            broadcaster.AddSubscriber(symbol, socket);
                        }
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        broadcaster.RemoveSubscriber(socket);
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
    }

}
