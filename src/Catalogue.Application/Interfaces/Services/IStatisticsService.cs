namespace Catalogue.Application.Interfaces.Services;

public interface IStatisticsService
{
    public IEnumerable<decimal> Numbers { get; }
    public int Quantity { get; }

    decimal Average();
    decimal Variance();
    decimal StandartDeviation();
    decimal Mode();
}