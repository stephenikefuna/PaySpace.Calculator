using Moq;

using NUnit.Framework;

using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests.Services.Calculators
{
    [TestFixture]
    internal sealed class ProgressiveCalculatorTests
    {
        
        [TestCase(-1, 0)]
        [TestCase(50, 5)]
        [TestCase(8350.1, 835.01)]
        [TestCase(8351, 835)]
        [TestCase(33951, 4674.85)]
        [TestCase(82251, 16749.60)]
        [TestCase(171550, 41753.32)]
        [TestCase(999999, 327681.79)]
        public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
        {
            // Arrange
            Mock<ICalculatorSettingsService> CalculatorSettingsServiceMock = new();
            var progressiveCalculator = new ProgressiveCalculator(CalculatorSettingsServiceMock.Object);

            var settings = GetCalculatorSettings();
            CalculatorSettingsServiceMock
                .Setup(x => x.GetSettingsAsync(CalculatorType.Progressive))
                .ReturnsAsync(settings)
                .Verifiable();

            // Act
            var actual = await progressiveCalculator.CalculateAsync(income);

            // Assert
            Assert.That(actual.Tax, Is.EqualTo(expectedTax));
        }

        private static List<CalculatorSetting> GetCalculatorSettings()
        {
            return
            [
                new() { Id = 1, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 10, From = 0, To = 8350 },
                new() { Id = 2, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 15, From = 8351, To = 33950 },
                new() { Id = 3, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 25, From = 33951, To = 82250 },
                new() { Id = 4, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 28, From = 82251, To = 171550 },
                new() { Id = 5, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 33, From = 171551, To = 372950 },
                new() { Id = 6, Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 35, From = 372951, To = null }
            ];
        }
    }
}