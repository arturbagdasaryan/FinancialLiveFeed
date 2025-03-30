using Microsoft.AspNetCore.Mvc;
using FinancialLiveFeed.Application.Interfaces;

namespace FinancialLiveFeed.WebAPI.Controllers;

[ApiController]
[Route("api/instruments")]
public class InstrumentsController : ControllerBase
{
    private readonly IInstrumentService _service;

    public InstrumentsController(IInstrumentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _service.GetAvailableInstrumentsAsync());

    [HttpGet("{symbol}/price")]
    public async Task<IActionResult> GetPrice(string symbol)
    {
        var price = await _service.GetCurrentPriceAsync(symbol);
        return price is null ? NotFound() : Ok(price);
    }
}
