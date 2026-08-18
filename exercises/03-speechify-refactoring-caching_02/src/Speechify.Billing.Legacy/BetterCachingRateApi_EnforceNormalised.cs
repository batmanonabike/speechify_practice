using Speechify.Billing.Abstractions;

namespace Speechify.Billing.Legacy;

public readonly struct NormalCurrencyCode(string currencyCode)
{
    public string Value { get; } = Normalise(currencyCode);

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
}

public class BetterCachingRateApi_EnforceNormalised : IRateApi
{
    private const int MinTtlMs = 10;
    private sealed record CacheEntry(decimal Rate, DateTimeOffset Timestamp);

    private readonly int _ttlMs;
    private readonly IRateApi _realApi;
    private readonly Lock _cacheGate = new();
    private readonly TimeProvider _timeProvider;
    private readonly Dictionary<string, CacheEntry> _cache = [];
    private readonly Dictionary<string, Lock> _concurrencyGates = [];

    public BetterCachingRateApi_EnforceNormalised(IRateApi realApi, TimeProvider timeProvider, int ttlMs)
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
        var normalCurrencyCode = new NormalCurrencyCode(currencyCode);
        var concurrencyGate = GetConcurrencyGate(normalCurrencyCode);

        lock (concurrencyGate)
        {
            if (TryGetCachedRate(normalCurrencyCode, out decimal cachedRate))
                return cachedRate;

            var rate = _realApi.GetUsdRate(normalCurrencyCode.Value);
            StoreCachedRate(normalCurrencyCode, rate);
            return rate;
        }
    }

    private bool TryGetCachedRate(in NormalCurrencyCode normalCurrencyCode, out decimal rate)
    {
        lock (_cacheGate)
        {
            if (_cache.TryGetValue(normalCurrencyCode.Value, out var existingCache) && !HasExpired(existingCache))
            {
                rate = existingCache.Rate;
                return true;
            }
        }

        rate = default;
        return false;
    }

    private void StoreCachedRate(in NormalCurrencyCode normalCurrencyCode, decimal rate)
    {
        var newCache = new CacheEntry(Rate: rate, _timeProvider.GetUtcNow());

        lock (_cacheGate)
            _cache[normalCurrencyCode.Value] = newCache;
    }

    private Lock GetConcurrencyGate(in NormalCurrencyCode normalCurrencyCode)
    {
        lock (_cacheGate)
        {
            if (_concurrencyGates.TryGetValue(normalCurrencyCode.Value, out var existingGate))
                return existingGate;

            var concurrencyGate = new Lock();
            _concurrencyGates[normalCurrencyCode.Value] = concurrencyGate;
            return concurrencyGate;
        }
    }

    private bool HasExpired(CacheEntry cacheEntry) =>
        (_timeProvider.GetUtcNow() - cacheEntry.Timestamp).TotalMilliseconds > _ttlMs;
}
