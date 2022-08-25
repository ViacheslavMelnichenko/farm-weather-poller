using FluentAssertions;

namespace Farm.Weather.Poller.UnitTests;

public class ConstantsTests
{
    [Fact]
    public void ConstantsShouldContainSpecificCitiesCount()
    {
        // Arrange
        // Act
        var constantsCount = Constants.Constants.UkrainianCities.Count();

        // Assert
        constantsCount.Should().Be(6);
    }
}