using Speechify.Billing.Abstractions;
using Speechify.Billing.Legacy;

namespace Speechify.Billing.Tests;

/// <summary>
/// In-memory rate provider. Counts calls so tests can prove caching works.
/// </summary>
public sealed class FakeRateApi : IRateApi
{
    private readonly Dictionary<string, decimal> _rates = new(StringComparer.OrdinalIgnoreCase)
    {
        ["USD"] = 1m,
        ["EUR"] = 1.11m,
        ["GBP"] = 1.27m,
        ["CAD"] = 0.74m,
        ["JPY"] = 0.0067m,
    };

    private int _callCount;

    /// <summary>Artificial latency, used to widen the window in concurrency tests.</summary>
    public int LatencyMs { get; set; }

    public int CallCount => Volatile.Read(ref _callCount);

    public void SetRate(string currencyCode, decimal rate) => _rates[currencyCode] = rate;

    public decimal GetUsdRate(string currencyCode)
    {
        Interlocked.Increment(ref _callCount);

        if (LatencyMs > 0)
        {
            Thread.Sleep(LatencyMs);
        }

        ArgumentNullException.ThrowIfNull(currencyCode);

        if (_rates.TryGetValue(currencyCode, out decimal rate))
        {
            return rate;
        }

        throw new ArgumentException("Unsupported currency: " + currencyCode, nameof(currencyCode));
    }
}

/// <summary>
/// A <see cref="TimeProvider"/> pinned to an instant you control.
/// </summary>
/// <remarks>
/// The local time zone is forced to UTC so that <see cref="TimeProvider.GetLocalNow"/>
/// and the legacy engine's <c>DateTime.Now</c> hook agree on the day of week.
/// </remarks>
public sealed class FixedTimeProvider : TimeProvider
{
    private DateTimeOffset _utcNow;

    public FixedTimeProvider(DateTimeOffset utcNow)
    {
        _utcNow = utcNow;
    }

    public override DateTimeOffset GetUtcNow() => _utcNow;

    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;

    public void Advance(TimeSpan by) => _utcNow = _utcNow.Add(by);
}

/// <summary>
/// Base class that scrubs the legacy engine's static globals between tests.
/// </summary>
/// <remarks>
/// <c>BillingEngine</c> holds a static rate dictionary and a static clock hook,
/// so tests leak into one another unless this runs. That leakage is itself one
/// of the defects you are being asked to remove.
/// </remarks>
public abstract class LegacyStateIsolated : IDisposable
{
    protected LegacyStateIsolated()
    {
        BillingEngine.ResetGlobalState();
    }

    public void Dispose()
    {
        BillingEngine.ResetGlobalState();
        GC.SuppressFinalize(this);
    }

    /// <summary>A Wednesday. No weekend surcharge.</summary>
    public static readonly DateTimeOffset Weekday = new(2026, 8, 19, 9, 0, 0, TimeSpan.Zero);

    /// <summary>A Saturday. Weekend surcharge applies.</summary>
    public static readonly DateTimeOffset Weekend = new(2026, 8, 22, 9, 0, 0, TimeSpan.Zero);

    /// <summary>Points the legacy engine's clock hook at a fixed instant.</summary>
    protected static void PinLegacyClock(DateTimeOffset instant)
    {
        BillingEngine.NowProvider = () => instant.UtcDateTime;
    }
}
