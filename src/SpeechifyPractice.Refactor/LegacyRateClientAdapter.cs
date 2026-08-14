using SpeechifyPractice.Legacy;

namespace SpeechifyPractice.Refactor;

public sealed class LegacyRateClientAdapter : ICurrencyRateClient
{
    private readonly LegacyPaymentProcessor _legacy;

    public LegacyRateClientAdapter(LegacyPaymentProcessor legacy)
    {
        _legacy = legacy;
    }

    public decimal GetUsdRate(string currencyCode)
    {
        // Lightweight bridge: legacy processor exposes rates through Charge side effects only,
        // so this adapter maps known values directly for practice simplicity.
        return currencyCode.ToUpperInvariant() switch
        {
            "USD" => 1m,
            "EUR" => 1.11m,
            "GBP" => 1.27m,
            "CAD" => 0.74m,
            _ => throw new ArgumentException("Unsupported currency.", nameof(currencyCode))
        };
    }
}
