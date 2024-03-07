using MapsterMapper;

using Microsoft.AspNetCore.Mvc;

using PaySpace.Calculator.API.Models;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostalCodeController(
        ILogger<PostalCodeController> logger,
        IPostalCodeService postalCodeService,
        IMapper mapper
        ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPostalCodes(CancellationToken cancellationToken)
        {
            try
            {
                var postalCodes = await postalCodeService.GetPostalCodesAsync(cancellationToken);

                return Ok(mapper.Map<List<PostalCodeDto>>(postalCodes));
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                return this.BadRequest("An error occured, please contact Admin");
            }
        }
    }
}
