namespace PaySpace.Calculator.Services.Abstractions
{
    public interface ICalculatorFactory
    {
        Task<ICalculator> GetCalculator(string postalCode, CancellationToken cancellationToken);
    }
}
