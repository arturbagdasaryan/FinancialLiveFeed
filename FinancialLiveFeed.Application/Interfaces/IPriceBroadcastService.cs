using System.Net.WebSockets;
using FinancialLiveFeed.Application.DTOs;

namespace FinancialLiveFeed.Application.Interfaces;

public interface IPriceBroadcastService
{
    void BroadcastPrice(PriceDto price);
    void AddSubscriber(string symbol, WebSocket socket);
    void RemoveSubscriber(WebSocket socket);
}
