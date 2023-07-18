using Farm.Weather.Contracts.Common;
using FluentAssertions;

namespace Farm.Weather.Poller.UnitTests;

public class ConstantsTests
{
    [Fact]
    public void ConstantsShouldContainSpecificCitiesCount()
    {
        // Arrange
        // Act
        var constantsCount = Constants.UkrainianCities.Count();

        // Assert
        constantsCount.Should().Be(8);
    }
}