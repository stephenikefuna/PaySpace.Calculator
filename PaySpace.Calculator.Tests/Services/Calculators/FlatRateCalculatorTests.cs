using Moq;

using NUnit.Framework;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;

using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests.Services.Calculators
{
    [TestFixture]
    internal sealed class FlatRateCalculatorTests
    {
        
        [TestCase(999999, 174999.825)]
        [TestCase(1000, 175)]
        [TestCase(5, 0.875)]
        public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
        {
            // Arrange
            Mock<ICalculatorSettingsService> CalculatorSettingsServiceMock = new();
            var flatRateCalculator = new FlatRateCalculator(CalculatorSettingsServiceMock.Object);

            var settings = GetCalculatorSettings();
            CalculatorSettingsServiceMock
                .Setup(x => x.GetSettingsAsync(CalculatorType.FlatRate))
                .ReturnsAsync(settings)
                .Verifiable();

            // Act
            var actual = await flatRateCalculator.CalculateAsync(income);

            // Assert
            Assert.That(actual.Tax, Is.EqualTo(expectedTax));
        }

        private static List<CalculatorSetting> GetCalculatorSettings()
        {
            return
            [
                new() { Id = 10, Calculator = CalculatorType.FlatRate, RateType = RateType.Percentage, Rate = 0, From = decimal.MinValue, To = -1 },
                new() { Id = 9, Calculator = CalculatorType.FlatRate, RateType = RateType.Percentage, Rate = 17.5M, From = 0, To = null }
            ];
        }
    }
}
