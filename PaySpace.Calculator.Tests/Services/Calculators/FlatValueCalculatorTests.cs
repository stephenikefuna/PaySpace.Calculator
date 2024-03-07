using Moq;

using NUnit.Framework;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;

using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests.Services.Calculators
{
    [TestFixture]
    internal sealed class FlatValueCalculatorTests
    {
        
        [TestCase(199999, 9999.95)]
        [TestCase(100, 5)]
        [TestCase(200000, 10000)]
        [TestCase(6000000, 10000)]
        public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
        {
            // Arrange
            Mock<ICalculatorSettingsService> CalculatorSettingsServiceMock = new();
            var flatValueCalculator = new FlatValueCalculator(CalculatorSettingsServiceMock.Object);

            var settings = GetCalculatorSettings();
            CalculatorSettingsServiceMock
                .Setup(x => x.GetSettingsAsync(CalculatorType.FlatValue))
                .ReturnsAsync(settings)
                .Verifiable();

            // Act
            var actual = await flatValueCalculator.CalculateAsync(income);

            // Assert
            Assert.That(actual.Tax, Is.EqualTo(expectedTax));
        }

        private static List<CalculatorSetting> GetCalculatorSettings()
        {
            return
            [
                new() { Id = 10, Calculator = CalculatorType.FlatValue, RateType = RateType.Percentage, Rate = 0, From = decimal.MinValue, To = -1 },
                new() { Id = 7, Calculator = CalculatorType.FlatValue, RateType = RateType.Percentage, Rate = 5, From = 0, To = 199999 },
                new() { Id = 8, Calculator = CalculatorType.FlatValue, RateType = RateType.Amount, Rate = 10000, From = 200000, To = null }
            ];
        }
    }
}