namespace SpeechifyPractice.Refactor;

public interface IClock
{
    DateTime UtcNow { get; }
}

public interface ICurrencyRateClient
{
    decimal GetUsdRate(string currencyCode);
}

public interface IFeeCalculator
{
    decimal Calculate(decimal amount, PaymentMethod method);
}
