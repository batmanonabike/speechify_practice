using Speechify.Billing.Abstractions;

namespace Speechify.Billing;

// ---------------------------------------------------------------------------
// This file is the OUTER BOUNDARY of your implementation, and nothing more.
//
// It exists only because the tests need something to call and something to
// inject fakes through. Everything behind it is your design: fee calculation,
// risk assessment, validation, currency normalisation, and the cache.
//
// There are deliberately NO interfaces here for a clock, a fee calculator or a
// rate cache. Choosing those seams is the exercise.
// ---------------------------------------------------------------------------

/// <summary>
/// A request to charge a customer.
/// </summary>
public sealed record ChargeRequest(
    decimal Amount,
    string Currency,
    string Method,
    string Country,
    string CustomerId,
    bool IsSubscription,
    int AccountAgeDays,
    string? PromoCode);

/// <summary>
/// The outcome of a charge. Field-for-field equivalent to the legacy receipt.
/// </summary>
public sealed record ChargeReceipt(
    decimal Amount,
    decimal Fee,
    decimal Surcharge,
    decimal TotalUsd,
    string RiskBand,
    string Currency);

/// <summary>
/// Replacement for <c>BillingEngine</c>. Must behave identically.
/// </summary>
public interface IBillingService
{
    ChargeReceipt Charge(ChargeRequest request);

    IReadOnlyList<ChargeReceipt> ChargeBatch(IEnumerable<ChargeRequest> requests);

    /// <summary>
    /// Fee shown to the customer before they commit.
    /// </summary>
    /// <remarks>
    /// Careful: the legacy <c>EstimateFee</c> does not agree with the fee that
    /// <c>ProcessCharge</c> actually applies. Characterize before you unify.
    /// </remarks>
    decimal EstimateFee(decimal amount, string method);
}

/// <summary>
/// Composition root. The tests build your service exclusively through this.
/// </summary>
public static class BillingComposition
{
    /// <summary>
    /// Wire up your implementation and return it.
    /// </summary>
    /// <param name="rateApi">The remote FX provider. Calls are slow and metered.</param>
    /// <param name="timeProvider">Use this for every time read. Never touch <c>DateTime.Now</c>.</param>
    /// <param name="rateTtl">How long a cached rate stays valid.</param>
    public static IBillingService Create(IRateApi rateApi, TimeProvider timeProvider, TimeSpan rateTtl)
    {
        throw new NotImplementedException(
            "Build your refactored billing service and return it from here.");
    }
}
