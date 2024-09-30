using Catalogue.Application.Interfaces.Services;

namespace Catalogue.Application.Services;

public class StatisticsService : IStatisticsService
{
    public IEnumerable<decimal> Numbers {get; private set;}  
    public int Quantity {get; private set;}

    public StatisticsService(IEnumerable<decimal> numbers)
    {
        Numbers = numbers;
        Quantity = numbers.Count();
    }

    public decimal Average()
    {
        decimal sum = 0;

        if(Numbers == null || Quantity <= 0 || !Numbers.Any())
            return sum;

        foreach(decimal num in Numbers)
        {
            sum += num;
        }

        return sum / Quantity;
    }

    public decimal Variance()
    {
        decimal variance = 0;
        
        if(Numbers == null || Quantity <= 0 || !Numbers.Any())
            return variance;

        decimal average = Average();

        foreach(decimal num in Numbers)
        {
            variance += (num - average) * (num - average);
        }

        return variance / Quantity;
    }

    public decimal StandartDeviation()
    {
        decimal standartDeviation = 0;

        if(Numbers == null || Quantity <= 0 || !Numbers.Any())
        {
            return standartDeviation;
        }

        standartDeviation = (decimal)Math.Sqrt((double)Variance());
        return standartDeviation;
    }

    public decimal Mode()
    {
        decimal mode = 0;

        if(Numbers == null || Quantity <= 0 || !Numbers.Any())
            return mode;

        var dict = new Dictionary<decimal, int>();

        foreach(decimal num in Numbers)
        {
            if(dict.TryGetValue(num, out int value))
            {
                dict[num] = value++;
            }
            else
            {
                dict.Add(num, 0);
            }
        }

        mode = dict.OrderByDescending(kvp => kvp.Value).First().Key;
        return mode;
    }
}