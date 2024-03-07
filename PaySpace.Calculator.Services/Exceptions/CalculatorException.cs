namespace PaySpace.Calculator.Services.Exceptions
{
    public sealed class CalculatorException : InvalidOperationException
    {
        public CalculatorException() : base("Invalid Postal code. Calculator not found") { }

        public CalculatorException(string message) : base(message) { }
    }
}