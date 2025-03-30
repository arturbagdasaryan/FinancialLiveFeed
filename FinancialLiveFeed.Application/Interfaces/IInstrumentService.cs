using FinancialLiveFeed.Application.DTOs;

namespace FinancialLiveFeed.Application.Interfaces;

public interface IInstrumentService
{
    Task<IEnumerable<InstrumentDto>> GetAvailableInstrumentsAsync();
    Task<PriceDto?> GetCurrentPriceAsync(string symbol);
}
