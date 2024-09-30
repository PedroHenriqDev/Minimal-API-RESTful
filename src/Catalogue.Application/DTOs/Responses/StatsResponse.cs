namespace Catalogue.Application.DTOs.Responses;

public class StatsResponse
{
    public decimal Average {get; set;}
    public decimal Variance { get; set; }
    public decimal Mode { get; set; }
    public decimal StandartDeviation {get; set;}
    public int Quantity {get; set;}

     public StatsResponse
    (   
        decimal average,
        decimal variance,
        decimal mode,
        int quantity,
        decimal standartDeviation 
    )
    {
        Average = average;
        Variance = variance;
        Mode = mode;
        Quantity = quantity;
        StandartDeviation = standartDeviation;
    }

    public StatsResponse()
    {
    }
}