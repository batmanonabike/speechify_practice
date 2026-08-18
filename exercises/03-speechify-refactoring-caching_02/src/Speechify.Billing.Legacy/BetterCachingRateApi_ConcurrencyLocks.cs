using Speechify.Billing.Abstractions;

namespace Speechify.Billing.Legacy;

public class BetterCachingRateApi_ConcurrencyLocks : IRateApi
{
    private const int MinTtlMs = 10;
    private sealed record CacheEntry(decimal Rate, DateTimeOffset Timestamp);

    private readonly int _ttlMs;
    private readonly IRateApi _realApi;
    private readonly Lock _cacheGate = new();
    private readonly TimeProvider _timeProvider;
    private readonly Dictionary<string, CacheEntry> _cache = [];
    private readonly Dictionary<string, Lock> _concurrencyGates = [];

    public BetterCachingRateApi_ConcurrencyLocks(IRateApi realApi, TimeProvider timeProvider, int ttlMs)
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
        var concurrencyGate = GetConcurrencyGate(currencyCode);

        lock (concurrencyGate)
        {
            if (TryGetCachedRate(currencyCode, out decimal cachedRate))
                return cachedRate;

            var rate = _realApi.GetUsdRate(currencyCode);
            StoreCachedRate(currencyCode, rate);
            return rate;
        }
    }

    private bool TryGetCachedRate(string currencyCode, out decimal rate)
    {
        var normalCurrencyCode = Normalise(currencyCode);
        lock (_cacheGate)
        {
            if (_cache.TryGetValue(normalCurrencyCode, out var existingCache) && !HasExpired(existingCache))
            {
                rate = existingCache.Rate;
                return true;
            }
        }

        rate = default;
        return false;
    }

    private void StoreCachedRate(string currencyCode, decimal rate)
    {
        var normalCurrencyCode = Normalise(currencyCode);
        var newCache = new CacheEntry(Rate: rate, _timeProvider.GetUtcNow());

        lock (_cacheGate)
            _cache[normalCurrencyCode] = newCache;
    }

    private Lock GetConcurrencyGate(string currencyCode)
    {
        var normalCurrencyCode = Normalise(currencyCode);

        lock (_cacheGate)
        {
            if (_concurrencyGates.TryGetValue(normalCurrencyCode, out var existingGate))
                return existingGate;

            var concurrencyGate = new Lock();
            _concurrencyGates[normalCurrencyCode] = concurrencyGate;
            return concurrencyGate;
        }
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
