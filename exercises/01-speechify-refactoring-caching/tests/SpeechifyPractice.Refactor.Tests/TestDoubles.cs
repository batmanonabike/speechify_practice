using SpeechifyPractice.Refactor;

namespace SpeechifyPractice.Refactor.Tests;

public sealed class FakeClock : IClock
{
    public FakeClock(DateTime utcNow)
    {
        UtcNow = utcNow;
    }

    public DateTime UtcNow { get; private set; }

    public void Advance(TimeSpan by)
    {
        UtcNow = UtcNow.Add(by);
    }
}

/// <summary>
/// Returns a single rate for every currency and counts calls.
/// </summary>
public sealed class FakeRateClient : ICurrencyRateClient
{
    private readonly decimal _rate;
    private int _callCount;

    public FakeRateClient(decimal rate)
    {
        _rate = rate;
    }

    public int CallCount => Volatile.Read(ref _callCount);

    public decimal GetUsdRate(string currencyCode)
    {
        Interlocked.Increment(ref _callCount);
        return _rate;
    }
}

/// <summary>
/// Mirrors the legacy rate table so refactored results can be compared with legacy ones.
/// </summary>
public sealed class TableRateClient : ICurrencyRateClient
{
    private int _callCount;

    public int CallCount => Volatile.Read(ref _callCount);

    public decimal GetUsdRate(string currencyCode)
    {
        Interlocked.Increment(ref _callCount);

        ArgumentNullException.ThrowIfNull(currencyCode);

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
