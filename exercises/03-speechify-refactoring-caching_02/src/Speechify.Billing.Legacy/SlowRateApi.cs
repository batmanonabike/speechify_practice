using Speechify.Billing.Abstractions;

namespace Speechify.Billing.Legacy;

/// <summary>
/// The production rate provider. Sleeps to stand in for a real network round trip.
/// </summary>
/// <remarks>
/// <see cref="CallCount"/> is what the caching work is measured against.
/// </remarks>
public sealed class SlowRateApi : IRateApi
{
    private readonly int _latencyMs;
    private int _callCount;

    public SlowRateApi(int latencyMs = 50)
    {
        _latencyMs = latencyMs;
    }

    public int CallCount => Volatile.Read(ref _callCount);

    public decimal GetUsdRate(string currencyCode)
    {
        Interlocked.Increment(ref _callCount);

        if (_latencyMs > 0)
        {
            Thread.Sleep(_latencyMs);
        }

        return currencyCode.ToUpperInvariant() switch
        {
            "USD" => 1m,
            "EUR" => 1.11m,
            "GBP" => 1.27m,
            "CAD" => 0.74m,
            "JPY" => 0.0067m,
            _ => throw new ArgumentException("Unsupported currency: " + currencyCode, nameof(currencyCode))
        };
    }
}
