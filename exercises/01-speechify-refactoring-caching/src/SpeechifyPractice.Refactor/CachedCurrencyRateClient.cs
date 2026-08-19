namespace SpeechifyPractice.Refactor;

public sealed class CachedCurrencyRateClient : ICurrencyRateClient
{
    private readonly ICurrencyRateClient _inner;
    private readonly IClock _clock;
    private readonly TimeSpan _ttl;

    public CachedCurrencyRateClient(ICurrencyRateClient inner, IClock clock, TimeSpan ttl)
    {
        _inner = inner;
        _clock = clock;
        _ttl = ttl;
    }

    public decimal GetUsdRate(string currencyCode)
    {
        // Practice task: cache by normalized currency code and expire entries by ttl.
        throw new NotImplementedException("Implement in-memory TTL cache decorator.");
    }
}
