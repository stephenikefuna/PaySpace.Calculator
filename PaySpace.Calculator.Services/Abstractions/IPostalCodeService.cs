using PaySpace.Calculator.Data.Models;

namespace PaySpace.Calculator.Services.Abstractions
{
    public interface IPostalCodeService
    {
        Task<List<PostalCode>> GetPostalCodesAsync(CancellationToken cancellationToken);

        Task<CalculatorType?> CalculatorTypeAsync(string code, CancellationToken cancellationToken);
    }
}