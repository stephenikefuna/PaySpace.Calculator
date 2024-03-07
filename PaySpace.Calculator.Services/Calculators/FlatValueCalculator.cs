using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class FlatValueCalculator(ICalculatorSettingsService calculatorSettingsService) : IFlatValueCalculator
    {
        public async Task<CalculateResult> CalculateAsync(decimal income)
        {
            var settings = (await calculatorSettingsService.GetSettingsAsync(CalculatorType.FlatValue))
                .Where(s => (income >= s.From)
                    && (!s.To.HasValue || income <= s.To))
                .FirstOrDefault() ?? throw new CalculatorException("No calculator setting found");

            return new CalculateResult
            {
                Calculator = settings.Calculator,
                Tax = settings.RateType == RateType.Amount 
                    ? settings.Rate 
                    : Helpers.CalculatePercentageTax(settings.Rate, income)
            };
        }
    }
}