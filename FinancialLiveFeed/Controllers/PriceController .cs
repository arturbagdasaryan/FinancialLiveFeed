using Microsoft.AspNetCore.Mvc;
using FinancialLiveFeed.Application.Interfaces;
using FinancialLiveFeed.Domain.Entities;

namespace FinancialLiveFeed.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly IInstrumentService _instrumentService;

        public PriceController(IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }

        [HttpGet("instruments")]
        public async Task<IActionResult> GetAvailableInstruments()
        {
            var instruments = await _instrumentService.GetAvailableInstrumentsAsync();
            return Ok(instruments);
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetCurrentPrice(string symbol)
        {
            var price = await _instrumentService.GetCurrentPriceAsync(symbol);
            return Ok(price);
        }
    }
}
