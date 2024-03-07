using PaySpace.Calculator.Data.Models;

namespace PaySpace.Calculator.Services
{
    public static class Helpers
    {
        public static decimal CalculatePercentageTax(decimal rate, decimal income)
        {
            return (rate/100) * income;
        }
    }
}
