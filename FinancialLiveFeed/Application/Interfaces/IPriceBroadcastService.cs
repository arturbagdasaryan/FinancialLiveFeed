using FinancialLiveFeed.Domain.Entities;
using System.Net.WebSockets;

namespace FinancialLiveFeed.Application.Interfaces
{
    public interface IPriceBroadcastService
    {
        void AddSubscriber(string symbol, WebSocket socket);
        void RemoveSubscriber(WebSocket socket);
        void BroadcastPrice(PriceUpdate priceUpdate);
    }
}
