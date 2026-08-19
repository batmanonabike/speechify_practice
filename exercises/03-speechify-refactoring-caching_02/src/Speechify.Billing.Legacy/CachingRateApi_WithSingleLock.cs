using Speechify.Billing.Abstractions;

namespace Speechify.Billing.Legacy;

public interface ISimpleCachingRateApi : IRateApi
{
    void ClearCache();
}

public class CachingRateApi_WithSingleLock : ISimpleCachingRateApi
{
    private const int MinTtlMs = 10;
    private sealed record CacheEntry(decimal Rate, DateTimeOffset Timestamp);

    private readonly int _ttlMs;
    private readonly IRateApi _realApi;
    private readonly Lock _cacheGate = new();
    private readonly TimeProvider _timeProvider;
    private readonly Dictionary<string, CacheEntry> _cache = [];

    public CachingRateApi_WithSingleLock(IRateApi realApi, TimeProvider timeProvider, int ttlMs)
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
        var normalised = Normalise(currencyCode);

        lock (_cacheGate)
        {
            if (_cache.TryGetValue(normalised, out var existingCache) && !HasExpired(existingCache))
                return existingCache.Rate;
        }

        var realTimeRate = _realApi.GetUsdRate(normalised);
        var newCache = new CacheEntry(Rate: realTimeRate, _timeProvider.GetUtcNow());

        lock (_cacheGate)
            _cache[normalised] = newCache;

        return newCache.Rate;
    }

    public void ClearCache()
    {
        lock (_cacheGate)
            _cache.Clear();
    }

    private bool HasExpired(CacheEntry cacheEntry) =>
        (_timeProvider.GetUtcNow() - cacheEntry.Timestamp).TotalMilliseconds > _ttlMs;

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

public static class SimpleCachingRateExtensions
{
    public static bool TryClearCache(this IRateApi rateApi)
    {
        ArgumentNullException.ThrowIfNull(rateApi);

        if (rateApi is not ISimpleCachingRateApi cachingRateApi)
            return false;

        cachingRateApi.ClearCache();
        return true;
    }
}
