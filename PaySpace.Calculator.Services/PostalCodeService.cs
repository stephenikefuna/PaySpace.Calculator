using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using PaySpace.Calculator.Data;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Services
{
    internal sealed class PostalCodeService(CalculatorContext context, IMemoryCache memoryCache) : IPostalCodeService
    {
        public Task<List<PostalCode>> GetPostalCodesAsync(CancellationToken cancellationToken)
        {
            return memoryCache.GetOrCreateAsync("PostalCodes", _ => context.Set<PostalCode>().AsNoTracking().ToListAsync(cancellationToken))!;
        }

        public async Task<CalculatorType?> CalculatorTypeAsync(string code,
            CancellationToken cancellationToken)
        {
            var postalCodes = await this.GetPostalCodesAsync(cancellationToken);

            var postalCode = postalCodes.FirstOrDefault(pc => pc.Code == code);

            return postalCode?.Calculator;
        }
    }
}