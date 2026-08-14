namespace SpeechifyPractice.Refactor;

public sealed record PaymentRequest(
	decimal Amount,
	string Currency,
	PaymentMethod Method,
	string Country,
	string CustomerId);

public enum PaymentMethod
{
	Card,
	BankTransfer,
	Wallet
}

public sealed record PaymentSummary(
	decimal Fee,
	decimal TotalUsd,
	string RiskBand);

public interface IClock
{
	DateTime UtcNow { get; }
}

public interface ICurrencyRateClient
{
	decimal GetUsdRate(string currencyCode);
}

public interface IFeeCalculator
{
	decimal Calculate(decimal amount, PaymentMethod method);
}

public sealed class PaymentFeeCalculator : IFeeCalculator
{
	public decimal Calculate(decimal amount, PaymentMethod method)
	{
		// Practice task: extract + simplify the legacy fee rules here.
		throw new NotImplementedException("Implement fee rules for Card, BankTransfer, and Wallet.");
	}
}

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

public sealed class PaymentCheckoutService
{
	private readonly IFeeCalculator _fees;
	private readonly ICurrencyRateClient _rates;

	public PaymentCheckoutService(IFeeCalculator fees, ICurrencyRateClient rates)
	{
		_fees = fees;
		_rates = rates;
	}

	public PaymentSummary Checkout(PaymentRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);

		// Practice task: orchestrate fee + rate + risk computation with clear, testable code.
		throw new NotImplementedException("Implement checkout orchestration.");
	}

	public static string ComputeRiskBand(decimal amount, string country, string customerId)
	{
		// Optional practice: refactor this logic for readability while preserving behavior.
		var score = 0;

		if (amount > 500m)
		{
			score += 2;
		}

		if (!string.Equals(country, "US", StringComparison.OrdinalIgnoreCase))
		{
			score += 1;
		}

		if (customerId.StartsWith("new_", StringComparison.OrdinalIgnoreCase))
		{
			score += 1;
		}

		if (score >= 3)
		{
			return "HIGH";
		}

		if (score == 2)
		{
			return "MEDIUM";
		}

		return "LOW";
	}
}
