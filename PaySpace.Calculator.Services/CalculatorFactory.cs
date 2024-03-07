using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.Services
{
    public class CalculatorFactory(IPostalCodeService postalCodeService,
        ICalculatorSettingsService calculatorSettingsService) : ICalculatorFactory
    {
        public async Task<ICalculator> GetCalculator(string postalCode,
            CancellationToken cancellationToken)
        {
            var calculatorType = await postalCodeService.CalculatorTypeAsync(postalCode, cancellationToken);

            return calculatorType switch
            {
                CalculatorType.FlatValue => new FlatValueCalculator(calculatorSettingsService),
                CalculatorType.Progressive => new ProgressiveCalculator(calculatorSettingsService),
                CalculatorType.FlatRate => new FlatRateCalculator(calculatorSettingsService),
                _ => throw new CalculatorException(),
            };
        }
    }
}
