namespace SpeechifyPractice.Refactor;

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
        // Optional practice: this is static, so it cannot be substituted in a test.
        // Extracting an IRiskAssessor is the SOLID move here — but preserve behaviour.
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
