using Moq;

using NUnit.Framework;

using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests.Services
{
    [TestFixture]
    internal sealed class CalculatorFactoryTests
    {

        [TestCase("7441", CalculatorType.Progressive)]
        [TestCase("1000", CalculatorType.Progressive)]
        [TestCase("A100", CalculatorType.FlatValue)]
        [TestCase("7000", CalculatorType.FlatRate)]
        public async Task GetCalculator_Should_Return_Expected_Calculator(string postCode,
            CalculatorType calculatorType)
        {
            // Arrange
            Mock<IPostalCodeService> PostalCodeServiceMock = new();
            Mock<ICalculatorSettingsService> CalculatorSettingsServiceMock = new();
            var calculatorFactory = new CalculatorFactory(PostalCodeServiceMock.Object,
                    CalculatorSettingsServiceMock.Object);

            var calculators = new Dictionary<CalculatorType, ICalculator>
            {
                {CalculatorType.Progressive, new ProgressiveCalculator(CalculatorSettingsServiceMock.Object) },
                {CalculatorType.FlatValue, new FlatValueCalculator(CalculatorSettingsServiceMock.Object) },
                {CalculatorType.FlatRate, new FlatRateCalculator(CalculatorSettingsServiceMock.Object) }
            };

            PostalCodeServiceMock
                .Setup(x => x.CalculatorTypeAsync(postCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(calculatorType)
                .Verifiable();


            // Act
            var result = await calculatorFactory.GetCalculator(postCode, CancellationToken.None);

            // Assert
            Assert.That(result, Is.TypeOf(calculators[calculatorType].GetType()));
        }
    }
}
