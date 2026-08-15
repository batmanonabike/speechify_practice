// ============================================================
// Unit Testing Patterns
// ============================================================
// This file is a CODE REFERENCE only — it won't be compiled
// into the CSharpRefresher console app because it depends on
// xUnit (which is in the test project, not here).
//
// See the real tests in:
//   exercises/01-speechify-refactoring-caching/tests/
//
// PATTERNS COVERED
//   - Arrange / Act / Assert (AAA)
//   - Fakes vs Mocks
//   - Test naming conventions
//   - Testing exceptions
//   - Parameterised tests ([Theory] + [InlineData])
//   - Testing async code
//   - Testing with IClock (time abstraction)
//   - What NOT to test
// ============================================================

/*
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpRefresher.Tests;

// ============================================================
// SYSTEM UNDER TEST — the code being tested
// ============================================================
public interface IPricer
{
    decimal GetPrice(string sku);
}

public class OrderCalculator(IPricer pricer)
{
    public decimal CalculateTotal(IEnumerable<string> skus)
    {
        decimal total = 0;
        foreach (var sku in skus)
            total += pricer.GetPrice(sku);
        return total;
    }
}

// ============================================================
// 1. AAA — Arrange, Act, Assert
// ============================================================
public class OrderCalculatorTests
{
    [Fact]
    public void CalculateTotal_SumsAllSkuPrices()
    {
        // Arrange — set up dependencies and inputs
        var fakePricer = new FakePricer(fixedPrice: 10m);
        var sut = new OrderCalculator(fakePricer);

        // Act — exercise the code under test
        decimal total = sut.CalculateTotal(["A", "B", "C"]);

        // Assert — verify the outcome
        Assert.Equal(30m, total);
    }
}

// ============================================================
// 2. Fakes — hand-written test doubles (preferred for simple cases)
// ============================================================
public class FakePricer(decimal fixedPrice) : IPricer
{
    public int CallCount { get; private set; }

    public decimal GetPrice(string sku)
    {
        CallCount++;
        return fixedPrice;
    }
}

// Fake with per-key prices
public class ConfigurablePricer : IPricer
{
    private readonly Dictionary<string, decimal> _prices;
    public ConfigurablePricer(Dictionary<string, decimal> prices) => _prices = prices;
    public decimal GetPrice(string sku) =>
        _prices.TryGetValue(sku, out var p) ? p : throw new KeyNotFoundException(sku);
}

// ============================================================
// 3. Test naming: MethodUnderTest_Scenario_ExpectedResult
// ============================================================
public class NamingExampleTests
{
    [Fact]
    public void GetPrice_UnknownSku_ThrowsKeyNotFoundException()
    {
        var sut = new ConfigurablePricer([]);
        Assert.Throws<KeyNotFoundException>(() => sut.GetPrice("UNKNOWN"));
    }

    [Fact]
    public void CalculateTotal_EmptySkuList_ReturnsZero()
    {
        var sut = new OrderCalculator(new FakePricer(99m));
        Assert.Equal(0m, sut.CalculateTotal([]));
    }
}

// ============================================================
// 4. Testing exceptions
// ============================================================
public class ExceptionTests
{
    [Fact]
    public void Method_WhenCondition_ThrowsExpectedExceptionType()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            throw new ArgumentException("bad input", "paramName"));

        Assert.Equal("paramName", ex.ParamName);    // assert on the exception itself
        Assert.Contains("bad input", ex.Message);
    }

    [Fact]
    public async Task AsyncMethod_WhenFails_ThrowsCorrectly()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await Task.Delay(1);
            throw new InvalidOperationException("async failure");
        });
    }
}

// ============================================================
// 5. Parameterised tests — [Theory] + [InlineData]
// ============================================================
public class ParameterisedTests
{
    // Test the same logic with multiple input/output pairs
    [Theory]
    [InlineData(100, "card",          3.20)]
    [InlineData(1000, "bank_transfer", 5.00)]   // capped
    [InlineData(200, "wallet",         3.00)]
    public void ComputeFee_ReturnsCorrectAmount(
        decimal amount, string method, decimal expectedFee)
    {
        // Arrange / Act / Assert condensed for data-driven tests
        decimal fee = ComputeFee(amount, method);
        Assert.Equal(expectedFee, fee);
    }

    private static decimal ComputeFee(decimal amount, string method) => method switch
    {
        "card"          => Math.Round(amount * 0.029m + 0.30m, 2),
        "bank_transfer" => Math.Min(Math.Round(amount * 0.01m, 2), 5m),
        "wallet"        => Math.Round(amount * 0.015m, 2),
        _               => throw new ArgumentException("Unknown method")
    };

    // [MemberData] for complex parameter types
    public static TheoryData<int[], int> SumData => new()
    {
        { new[] {1,2,3}, 6 },
        { new[] {0},     0 },
        { Array.Empty<int>(), 0 },
    };

    [Theory, MemberData(nameof(SumData))]
    public void Sum_ReturnsCorrectTotal(int[] values, int expected)
        => Assert.Equal(expected, values.Sum());
}

// ============================================================
// 6. Testing async code
// ============================================================
public class AsyncTests
{
    [Fact]
    public async Task FetchAsync_ReturnsExpectedValue()
    {
        // Just make the test method async Task and await normally
        var result = await Task.FromResult(42);
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task WithCancellation_ThrowsOperationCancelledException()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await Task.Delay(1000, cts.Token));
    }
}

// ============================================================
// 7. Testing with a clock abstraction (IClock)
// ============================================================
public interface IClock2 { DateTime UtcNow { get; } }

public class FakeClock(DateTime initial) : IClock2
{
    public DateTime UtcNow { get; private set; } = initial;
    public void Advance(TimeSpan by) => UtcNow = UtcNow.Add(by);
}

public class TokenStore(IClock2 clock, TimeSpan ttl)
{
    private string? _token;
    private DateTime _expiresAt;

    public void Store(string token)
    {
        _token = token;
        _expiresAt = clock.UtcNow.Add(ttl);
    }

    public string? Get() => clock.UtcNow < _expiresAt ? _token : null;
}

public class TokenStoreTests
{
    [Fact]
    public void Get_WithinTtl_ReturnsToken()
    {
        var clock = new FakeClock(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        var store = new TokenStore(clock, TimeSpan.FromMinutes(5));

        store.Store("abc123");
        clock.Advance(TimeSpan.FromMinutes(4));   // still within TTL

        Assert.Equal("abc123", store.Get());
    }

    [Fact]
    public void Get_AfterTtlExpires_ReturnsNull()
    {
        var clock = new FakeClock(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        var store = new TokenStore(clock, TimeSpan.FromMinutes(5));

        store.Store("abc123");
        clock.Advance(TimeSpan.FromMinutes(6));   // TTL expired

        Assert.Null(store.Get());
    }
}

// ============================================================
// 8. What NOT to test
// ============================================================
// - Third-party library behaviour (trust the library)
// - Private methods directly (test via public surface)
// - Constructor parameter assignment (test behaviour, not wiring)
// - Auto-properties with no logic
// - ToString / Equals on records (compiler-generated, trusted)
*/
