using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaySpace.Calculator.Web.Models
{
    public sealed class CalculatorViewModel
    {
        public IEnumerable<SelectListItem> PostalCodes { get; set; }

        public string PostalCode { get; set; }

        public decimal Income { get; set; }
    }
}