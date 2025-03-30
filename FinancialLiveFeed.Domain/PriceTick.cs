namespace FinancialLiveFeed.Domain.Entities;

public class PriceTick
{
    public string Symbol { get; set; } = default!;
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
}
