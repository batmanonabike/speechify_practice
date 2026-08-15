// ============================================================
// Design Patterns — Decorator, Strategy, Adapter
// ============================================================
// These three patterns are directly present in the Speechify
// practice exercise:
//
//   Adapter   — LegacyRateClientAdapter wraps LegacyPaymentProcessor
//   Decorator — CachedCurrencyRateClient wraps ICurrencyRateClient
//   Strategy  — IFeeCalculator swapped per payment method
//
// ============================================================

using System;
using System.Collections.Generic;

namespace CSharpRefresher;

// ============================================================
// ADAPTER
// ============================================================
// Converts the interface of an existing class into the interface
// a client expects.  "Make incompatible things work together."
//
// Structure:
//   ITarget       — what the client wants to talk to
//   Adaptee       — existing class with an incompatible interface
//   Adapter       — wraps Adaptee, implements ITarget
// ============================================================

// What our modern code expects
public interface ITemperatureService
{
    double GetTemperatureCelsius(string city);
}

// Legacy system that only speaks Fahrenheit — we can't change it
public class LegacyWeatherApi
{
    public double FetchFahrenheit(string location)
    {
        // Simulated: always returns 77°F
        Console.WriteLine($"[LegacyWeatherApi] fetching for {location}");
        return 77.0;
    }
}

// Adapter: wraps the legacy API and converts units
public class WeatherApiAdapter(LegacyWeatherApi legacy) : ITemperatureService
{
    public double GetTemperatureCelsius(string city)
    {
        double fahrenheit = legacy.FetchFahrenheit(city);
        return Math.Round((fahrenheit - 32) * 5.0 / 9.0, 2);
    }
}

// ============================================================
// DECORATOR
// ============================================================
// Adds behaviour to an object dynamically without subclassing.
// Wraps the original behind the same interface.
// Decorators can be stacked (each wraps the previous).
//
// Structure:
//   IComponent        — shared interface
//   ConcreteComponent — the real implementation
//   Decorator         — wraps IComponent, adds behaviour, delegates the rest
// ============================================================

public interface IMessageSender
{
    void Send(string message);
}

public class EmailSender : IMessageSender
{
    public void Send(string message) =>
        Console.WriteLine($"[Email] {message}");
}

// Decorator 1: adds logging
public class LoggingMessageSender(IMessageSender inner) : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine($"[LOG] Sending: {message}");
        inner.Send(message);
        Console.WriteLine($"[LOG] Sent.");
    }
}

// Decorator 2: adds retry
public class RetryMessageSender(IMessageSender inner, int maxAttempts = 3) : IMessageSender
{
    public void Send(string message)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                inner.Send(message);
                return;
            }
            catch (Exception ex) when (attempt < maxAttempts)
            {
                Console.WriteLine($"[RETRY] attempt {attempt} failed: {ex.Message}");
            }
        }
    }
}

// Decorator 3: TTL cache (mirrors CachedCurrencyRateClient in the exercise)
public interface IExchangeRateService
{
    decimal GetRate(string currency);
}

public class LiveExchangeRateService : IExchangeRateService
{
    public int CallCount { get; private set; }

    public decimal GetRate(string currency)
    {
        CallCount++;
        Console.WriteLine($"[Live] fetching rate for {currency}");
        return currency.ToUpperInvariant() switch
        {
            "EUR" => 1.11m,
            "GBP" => 1.27m,
            _     => 1.00m,
        };
    }
}

public class CachedExchangeRateService(
    IExchangeRateService inner,
    TimeSpan ttl) : IExchangeRateService
{
    private readonly record struct CacheEntry(decimal Rate, DateTime ExpiresAt);
    private readonly Dictionary<string, CacheEntry> _cache = [];

    public decimal GetRate(string currency)
    {
        string key = currency.ToUpperInvariant();
        var now = DateTime.UtcNow;

        if (_cache.TryGetValue(key, out var entry) && now < entry.ExpiresAt)
        {
            Console.WriteLine($"[Cache] HIT for {key}");
            return entry.Rate;
        }

        decimal rate = inner.GetRate(key);
        _cache[key] = new CacheEntry(rate, now.Add(ttl));
        return rate;
    }
}

// ============================================================
// STRATEGY
// ============================================================
// Defines a family of algorithms, encapsulates each one, and
// makes them interchangeable.  The client chooses the strategy
// at runtime without knowing the implementation details.
//
// Structure:
//   IStrategy         — algorithm interface
//   ConcreteStrategyA — one algorithm
//   ConcreteStrategyB — another algorithm
//   Context           — holds a reference to IStrategy, delegates to it
// ============================================================

public interface ISortStrategy<T>
{
    void Sort(List<T> items);
}

public class BubbleSortStrategy<T> : ISortStrategy<T> where T : IComparable<T>
{
    public void Sort(List<T> items)
    {
        Console.WriteLine("[BubbleSort] sorting...");
        for (int i = 0; i < items.Count - 1; i++)
            for (int j = 0; j < items.Count - 1 - i; j++)
                if (items[j].CompareTo(items[j + 1]) > 0)
                    (items[j], items[j + 1]) = (items[j + 1], items[j]);
    }
}

public class LinqSortStrategy<T> : ISortStrategy<T> where T : IComparable<T>
{
    public void Sort(List<T> items)
    {
        Console.WriteLine("[LinqSort] sorting...");
        var sorted = items.OrderBy(x => x).ToList();
        items.Clear();
        items.AddRange(sorted);
    }
}

public class Sorter<T>(ISortStrategy<T> strategy) where T : IComparable<T>
{
    private ISortStrategy<T> _strategy = strategy;

    // Strategy can be swapped at runtime
    public void SetStrategy(ISortStrategy<T> strategy) => _strategy = strategy;

    public void Sort(List<T> items) => _strategy.Sort(items);
}

// Real-world strategy: fee calculation (mirrors the exercise)
public interface IFeeStrategy
{
    decimal Calculate(decimal amount);
}

public class CardFeeStrategy : IFeeStrategy
{
    public decimal Calculate(decimal amount) =>
        Math.Round(amount * 0.029m + 0.30m, 2, MidpointRounding.AwayFromZero);
}

public class BankTransferFeeStrategy : IFeeStrategy
{
    public decimal Calculate(decimal amount) =>
        Math.Round(Math.Min(amount * 0.01m, 5m), 2, MidpointRounding.AwayFromZero);
}

public class WalletFeeStrategy : IFeeStrategy
{
    public decimal Calculate(decimal amount) =>
        Math.Round(amount * 0.015m, 2, MidpointRounding.AwayFromZero);
}

public static class DesignPatternsExamples
{
    public static void Run()
    {
        Console.WriteLine("=== ADAPTER ===");
        var legacy  = new LegacyWeatherApi();
        ITemperatureService service = new WeatherApiAdapter(legacy);
        Console.WriteLine($"Temperature: {service.GetTemperatureCelsius("London")}°C");

        Console.WriteLine("\n=== DECORATOR (stacked) ===");
        // Build up the decoration chain: Email → Logging → Retry
        IMessageSender sender =
            new RetryMessageSender(
                new LoggingMessageSender(
                    new EmailSender()));
        sender.Send("Hello from decorators!");

        Console.WriteLine("\n=== DECORATOR (TTL cache) ===");
        var live    = new LiveExchangeRateService();
        var cached  = new CachedExchangeRateService(live, TimeSpan.FromMinutes(5));

        cached.GetRate("EUR");   // miss → calls live
        cached.GetRate("EUR");   // hit  → from cache
        cached.GetRate("GBP");   // miss → calls live
        cached.GetRate("eur");   // hit  → normalised key "EUR"
        Console.WriteLine($"Live service called {live.CallCount} time(s)");   // 2

        Console.WriteLine("\n=== STRATEGY (sort) ===");
        var data = new List<int> { 5, 3, 1, 4, 2 };
        var sorter = new Sorter<int>(new BubbleSortStrategy<int>());
        sorter.Sort(data);
        Console.WriteLine(string.Join(", ", data));

        sorter.SetStrategy(new LinqSortStrategy<int>());
        var data2 = new List<int> { 9, 7, 8, 6 };
        sorter.Sort(data2);
        Console.WriteLine(string.Join(", ", data2));

        Console.WriteLine("\n=== STRATEGY (fees — mirrors exercise) ===");
        var strategies = new Dictionary<string, IFeeStrategy>
        {
            ["card"]          = new CardFeeStrategy(),
            ["bank_transfer"] = new BankTransferFeeStrategy(),
            ["wallet"]        = new WalletFeeStrategy(),
        };

        foreach (var (method, strategy) in strategies)
            Console.WriteLine($"{method,-14}: fee on $100 = {strategy.Calculate(100m):F2}");
    }
}
