using FinancialLiveFeed.Application.Interfaces;
using FinancialLiveFeed.Domain.Entities;

namespace FinancialLiveFeed.Infrastructure.Services
{
    public class InstrumentService : IInstrumentService
    {
        private readonly List<FinancialInstrument> _instruments = new()
    {
        new FinancialInstrument { Symbol = "EURUSD", Name = "Euro to USD" },
        new FinancialInstrument { Symbol = "USDJPY", Name = "USD to JPY" },
        new FinancialInstrument { Symbol = "BTCUSD", Name = "Bitcoin to USD" }
    };

        public Task<IEnumerable<FinancialInstrument>> GetAvailableInstrumentsAsync()
        {
            return Task.FromResult(_instruments.AsEnumerable());
        }

        public Task<decimal> GetCurrentPriceAsync(string symbol)
        {
            // Simulate fetching live price from external source.
            return Task.FromResult(symbol switch
            {
                "EURUSD" => 1.12M,
                "USDJPY" => 110.50M,
                "BTCUSD" => 50000.00M,
                _ => 0M,
            });
        }
    }
}
