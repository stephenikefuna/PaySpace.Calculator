using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class FlatRateCalculator(ICalculatorSettingsService calculatorSettingsService) : IFlatRateCalculator
    {
        public async Task<CalculateResult> CalculateAsync(decimal income)
        {
            var settings = (await calculatorSettingsService.GetSettingsAsync(Data.Models.CalculatorType.FlatRate))
                .Where(s => (income >= s.From)
                    && (!s.To.HasValue || income <= s.To))
                .FirstOrDefault() ?? throw new CalculatorException("No calculator setting found");

            return new CalculateResult
            {
                Calculator = settings.Calculator,
                Tax = Helpers.CalculatePercentageTax(settings.Rate, income)
            };
        }
    }
}