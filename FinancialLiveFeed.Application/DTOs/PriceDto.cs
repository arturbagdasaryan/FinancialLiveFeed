namespace FinancialLiveFeed.Application.DTOs;

public class PriceDto
{
    public string Symbol { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
}
