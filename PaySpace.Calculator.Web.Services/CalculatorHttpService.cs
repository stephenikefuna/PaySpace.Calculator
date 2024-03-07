using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Options;

using PaySpace.Calculator.Web.Services.Abstractions;
using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Services
{
    public class CalculatorHttpService : ICalculatorHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly CalculatorSettings _calculatorSettings;

        public CalculatorHttpService(IHttpClientFactory httpClient,
            IOptions<CalculatorSettings> options)
        {
            _httpClient = httpClient.CreateClient();
            _calculatorSettings = options.Value;
            _httpClient.BaseAddress = new Uri(_calculatorSettings.ApiUrl);
        }

        public async Task<List<PostalCode>> GetPostalCodesAsync()
        {
            var response = await _httpClient.GetAsync("api/postalcode");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch postal codes, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<PostalCode>>() ?? [];
        }

        public async Task<List<CalculatorHistory>> GetHistoryAsync()
        {
            var response = await _httpClient.GetAsync("api/calculator/history");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot fetch calculator history, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<CalculatorHistory>>() ?? [];
        }

        public async Task<CalculateResult> CalculateTaxAsync(CalculateRequest calculationRequest)
        {
            var content = new StringContent(JsonSerializer.Serialize(calculationRequest),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/calculator/calculate-tax",
                content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot calculate tax, status code: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<CalculateResult>() 
                ?? throw new Exception($"Cannot calculate tax, status code: {response.StatusCode}");
        }
    }
}