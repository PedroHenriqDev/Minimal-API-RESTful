using Catalogue.Application.Interfaces.Services;
using Catalogue.Application.Services;

namespace Catalogue.UnitTests.Services;

public class StatisticsTests
{
    private readonly IStatisticsService _statsService;

    public StatisticsTests()
    {
        _statsService = new StatisticsService(new List<decimal>
        {
            80,
            20,
            20
        });
    }

    [Fact]
    public void Average_WhenCalledWithListOfDecimal_ReturnExpectedAverage()
    {
        //Arrange
        decimal averageExpected = 40;

        //Act
        decimal result = _statsService.Average();

        //Assert
        Assert.Equal(averageExpected, result);
    }

    [Fact]
    public void Mode_WhenCalledWithListOfDecimal_ReturnExpectedMode()
    {
        //Arrange
        decimal modeExpected = 20;

        //Act
        decimal result = _statsService.Mode();

        //Assert
        Assert.Equal(modeExpected, result);
    }

    [Fact]
    public void Variance_WhenCalledWithListOfDecimal_ReturnExpectedVariance()
    {
        //Arrange
        decimal varianceExpected = 800;

        //Act
        decimal result = _statsService.Variance();

        //Assert
        Assert.Equal(varianceExpected, result);
    }

    [Fact]
    public void StandartDeviation_WhenCalledWithListOfDecimal_ReturnExpectedStandartDeviation()
    {
        //Arrange
        decimal standartDeviationExpected = 28.28m;

        //Act
        decimal result = _statsService.StandartDeviation();

        //Assert
        Assert.Equal(standartDeviationExpected, result, precision: 2);
    }

    [Fact]
    public void Quantity_WhenCalledWithListOfDecimal_ReturnExpectedQuantity()
    {
        //Arrange
        int quantityExpected = 3;

        //Act & Assert
        Assert.Equal(quantityExpected, _statsService.Quantity);
    }
}