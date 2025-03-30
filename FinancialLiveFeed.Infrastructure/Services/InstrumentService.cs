using FinancialLiveFeed.Application.DTOs;
using FinancialLiveFeed.Application.Interfaces;

namespace FinancialLiveFeed.Infrastructure.Services;

public class InstrumentService : IInstrumentService
{
    private static readonly List<InstrumentDto> _instruments = new()
    {
        new InstrumentDto { Symbol = "BTCUSD", Name = "Bitcoin / USD" },
        new InstrumentDto { Symbol = "EURUSD", Name = "Euro / USD" },
        new InstrumentDto { Symbol = "USDJPY", Name = "USD / Yen" }
    };

    private readonly Dictionary<string, PriceDto> _cache;

    public InstrumentService(Dictionary<string, PriceDto> cache) => _cache = cache;

    public Task<IEnumerable<InstrumentDto>> GetAvailableInstrumentsAsync() => Task.FromResult(_instruments.AsEnumerable());

    public Task<PriceDto?> GetCurrentPriceAsync(string symbol)
        => Task.FromResult(_cache.TryGetValue(symbol.ToUpper(), out var p) ? p : null);
}
