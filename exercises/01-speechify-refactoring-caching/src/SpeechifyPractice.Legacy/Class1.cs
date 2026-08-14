namespace SpeechifyPractice.Legacy;

public sealed record LegacyPaymentRequest(
	decimal Amount,
	string Currency,
	string PaymentMethod,
	string Country,
	string CustomerId);

public sealed record LegacyPaymentReceipt(
	decimal OriginalAmount,
	decimal Fee,
	decimal TotalChargedUsd,
	string RiskBand);

public sealed class LegacyPaymentProcessor
{
	public int ExchangeRateLookupCount { get; private set; }

	public LegacyPaymentReceipt Charge(LegacyPaymentRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var fee = ComputeFee(request.Amount, request.PaymentMethod);
		var riskBand = ComputeRiskBand(request.Amount, request.Country, request.CustomerId);

		var total = request.Amount + fee;
		var usdRate = GetUsdRate(request.Currency);
		var totalUsd = Math.Round(total * usdRate, 2, MidpointRounding.AwayFromZero);

		return new LegacyPaymentReceipt(request.Amount, fee, totalUsd, riskBand);
	}

	private decimal ComputeFee(decimal amount, string paymentMethod)
	{
		if (string.Equals(paymentMethod, "card", StringComparison.OrdinalIgnoreCase))
		{
			return Math.Round((amount * 0.029m) + 0.30m, 2, MidpointRounding.AwayFromZero);
		}

		if (string.Equals(paymentMethod, "bank_transfer", StringComparison.OrdinalIgnoreCase))
		{
			var fee = amount * 0.01m;
			if (fee > 5m)
			{
				fee = 5m;
			}

			return Math.Round(fee, 2, MidpointRounding.AwayFromZero);
		}

		if (string.Equals(paymentMethod, "wallet", StringComparison.OrdinalIgnoreCase))
		{
			return Math.Round(amount * 0.015m, 2, MidpointRounding.AwayFromZero);
		}

		throw new ArgumentException("Unknown payment method.", nameof(paymentMethod));
	}

	private static string ComputeRiskBand(decimal amount, string country, string customerId)
	{
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

	private decimal GetUsdRate(string currency)
	{
		ExchangeRateLookupCount++;

		return currency.ToUpperInvariant() switch
		{
			"USD" => 1m,
			"EUR" => 1.11m,
			"GBP" => 1.27m,
			"CAD" => 0.74m,
			_ => throw new ArgumentException("Unsupported currency.", nameof(currency))
		};
	}
}
