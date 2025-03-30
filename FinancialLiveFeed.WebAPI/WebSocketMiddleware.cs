using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using FinancialLiveFeed.Application.Interfaces;

namespace FinancialLiveFeed.WebAPI.Middleware;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<WebSocketMiddleware> _logger;

    public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IPriceBroadcastService broadcaster)
    {
        if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
        {
            using var socket = await context.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[4096];
            _logger.LogInformation("WebSocket client connected");

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var doc = JsonDocument.Parse(Encoding.UTF8.GetString(buffer, 0, result.Count));
                    if (doc.RootElement.TryGetProperty("symbol", out var s) &&
                        doc.RootElement.TryGetProperty("type", out var t) &&
                        t.GetString() == "subscribe")
                    {
                        broadcaster.AddSubscriber(s.GetString()!, socket);
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    broadcaster.RemoveSubscriber(socket);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", CancellationToken.None);
                }
            }
        }
        else await _next(context);
    }
}
