namespace FinancialLiveFeed.Application.Interfaces;

public interface IPriceFeedClient
{
    Task StartAsync(CancellationToken cancellationToken);
}
