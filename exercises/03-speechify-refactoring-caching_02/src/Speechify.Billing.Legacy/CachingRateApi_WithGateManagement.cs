using Speechify.Billing.Abstractions;

namespace Speechify.Billing.Legacy;

public class CachingRateApi_WithGateManagement : IRateApi
{
    private const int MinTtlMs = 10;
    private sealed record CacheEntry(decimal Rate, DateTimeOffset Timestamp);

    private sealed class ConcurrencyGate(string normalCurrencyCode)
    {
        public int RefCount { get; set; }
        public Lock Gate { get; } = new();
        public string NormalCurrencyCode { get; } = normalCurrencyCode;
    }

    private readonly int _ttlMs;
    private readonly IRateApi _realApi;
    private readonly Lock _cacheGate = new();
    private readonly TimeProvider _timeProvider;
    private readonly Dictionary<string, CacheEntry> _cache = [];
    private readonly Dictionary<string, ConcurrencyGate> _concurrencyGates = [];

    public CachingRateApi_WithGateManagement(IRateApi realApi, TimeProvider timeProvider, int ttlMs)
    {
        ArgumentNullException.ThrowIfNull(realApi);
        ArgumentNullException.ThrowIfNull(timeProvider);
        if (ttlMs < MinTtlMs) throw new ArgumentException($"Unsupported TTL: {ttlMs}ms", nameof(ttlMs));

        _ttlMs = ttlMs;
        _realApi = realApi;
        _timeProvider = timeProvider;
    }

    public decimal GetUsdRate(string currencyCode)
    {
        var concurrencyGate = AcquireConcurrencyGate(currencyCode);

        try
        {
            lock (concurrencyGate.Gate)
            {
                if (TryGetCachedRate(concurrencyGate, out decimal cachedRate))
                    return cachedRate;

                var rate = _realApi.GetUsdRate(concurrencyGate.NormalCurrencyCode);
                StoreCachedRate(concurrencyGate, rate);
                return rate;
            }
        }
        finally
        {
            ReleaseConcurrencyGate(concurrencyGate);
        }
    }

    private bool TryGetCachedRate(ConcurrencyGate concurrencyGate, out decimal rate)
    {
        lock (_cacheGate)
        {
            if (_cache.TryGetValue(concurrencyGate.NormalCurrencyCode, out var existingCache) && !HasExpired(existingCache))
            {
                rate = existingCache.Rate;
                return true;
            }
        }

        rate = default;
        return false;
    }

    private ConcurrencyGate AcquireConcurrencyGate(string currencyCode)
    {
        var normalCurrencyCode = Normalise(currencyCode);

        lock (_cacheGate)
        {
            if (!_concurrencyGates.TryGetValue(normalCurrencyCode, out var existingGate))
            {
                existingGate = new ConcurrencyGate(normalCurrencyCode);
                _concurrencyGates[normalCurrencyCode] = existingGate;
            }

            existingGate.RefCount++;
            return existingGate;
        }
    }

    private void ReleaseConcurrencyGate(ConcurrencyGate concurrencyGate)
    {
        lock (_cacheGate)
        {
            concurrencyGate.RefCount--;

            if (concurrencyGate.RefCount == 0)
            {
                if (_concurrencyGates.TryGetValue(concurrencyGate.NormalCurrencyCode, out var value) &&
                    ReferenceEquals(concurrencyGate, value))
                {
                    _concurrencyGates.Remove(concurrencyGate.NormalCurrencyCode);
                }
            }
        }
    }

    private void StoreCachedRate(ConcurrencyGate concurrencyGate, decimal rate)
    {
        var newCache = new CacheEntry(Rate: rate, _timeProvider.GetUtcNow());

        lock (_cacheGate)
            _cache[concurrencyGate.NormalCurrencyCode] = newCache;
    }

    private static string Normalise(string currencyCode)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(currencyCode);

        return currencyCode.ToUpperInvariant().Trim() switch
        {
            "USD" => "USD",
            "EUR" => "EUR",
            "GBP" => "GBP",
            "CAD" => "CAD",
            "JPY" => "JPY",
            _ => throw new ArgumentException($"Unsupported currency: {currencyCode}", nameof(currencyCode))
        };
    }

    private bool HasExpired(CacheEntry cacheEntry) =>
        (_timeProvider.GetUtcNow() - cacheEntry.Timestamp).TotalMilliseconds > _ttlMs;
}
