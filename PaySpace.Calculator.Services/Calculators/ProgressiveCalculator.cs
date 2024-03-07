using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Exceptions;
using PaySpace.Calculator.Services.Models;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class ProgressiveCalculator(ICalculatorSettingsService calculatorSettingsService) : IProgressiveCalculator
    {
        public async Task<CalculateResult> CalculateAsync(decimal income)
        {
            var settings = (await calculatorSettingsService.GetSettingsAsync(CalculatorType.Progressive))
                .Where(s => (income >= s.From)
                    && (!s.To.HasValue || income <= s.To))
                .OrderBy(s => s.From)
                .ToList() ?? throw new CalculatorException("No calculator setting found");

            return new CalculateResult
            {
                Calculator = CalculatorType.Progressive,
                Tax = CalculatePercentageTax(income, settings)
            };
        }

        private decimal CalculatePercentageTax(decimal income, List<CalculatorSetting> settings)
        {
            decimal remainder = income;
            decimal tax = 0;

            foreach (var setting in settings)
            {
                if (remainder < 1)
                {
                    return tax;
                }

                decimal deficit = setting.To ?? 0 - setting.From;
                decimal taxable = remainder >= deficit ? deficit : remainder;
                tax += (setting.Rate / 100) * taxable;
                remainder -= taxable;
            }

            return tax;
        }
    }
}