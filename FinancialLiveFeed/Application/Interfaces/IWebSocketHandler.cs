using System.Net.WebSockets;
namespace FinancialLiveFeed.Application.Interfaces
{
    public interface IWebSocketHandler
    {
        Task HandleWebSocketConnection(WebSocket webSocket);
    }
}
