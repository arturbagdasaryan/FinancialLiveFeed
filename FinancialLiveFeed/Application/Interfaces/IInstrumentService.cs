using FinancialLiveFeed.Domain.Entities;

namespace FinancialLiveFeed.Application.Interfaces
{
    public interface IInstrumentService
    {
        Task<IEnumerable<FinancialInstrument>> GetAvailableInstrumentsAsync();
        Task<decimal> GetCurrentPriceAsync(string symbol);
    }
}
