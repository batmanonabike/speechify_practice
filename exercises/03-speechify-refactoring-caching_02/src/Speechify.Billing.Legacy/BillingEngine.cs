using Speechify.Billing.Abstractions;

namespace Speechify.Billing.Legacy;

/// <summary>
/// Charges a customer and produces a receipt.
/// </summary>
/// <remarks>
/// This class is in production and its output is correct as far as anyone knows.
/// It is the only definition of "correct" you have — treat it as the oracle.
/// Do not edit it. Characterize it, then replace it.
/// </remarks>
public class BillingEngine
{
    // Rates are cached here so we don't hammer the FX provider.
    // Added under time pressure during the Black Friday incident. Works fine.
    private static readonly Dictionary<string, decimal> RateCache = new Dictionary<string, decimal>();

    /// <summary>
    /// Hook so the billing tests can pin "now". Do not use in production code.
    /// </summary>
    public static Func<DateTime> NowProvider = () => DateTime.Now;

    private readonly IRateApi _rateApi;

    public BillingEngine(IRateApi rateApi)
    {
        _rateApi = rateApi;
    }

    /// <summary>
    /// Number of times this instance went out to the rate provider.
    /// </summary>
    public int RateLookupCount { get; private set; }

    /// <summary>
    /// Only exists because the statics above make this class impossible to test
    /// otherwise. Call between tests.
    /// </summary>
    public static void ResetGlobalState()
    {
        lock (RateCache)
        {
            RateCache.Clear();
        }

        NowProvider = () => DateTime.Now;
    }

    public LegacyReceipt ProcessCharge(
        decimal amount,
        string currency,
        string method,
        string country,
        string customerId,
        bool isSubscription,
        int accountAgeDays,
        string promoCode)
    {
        if (currency == null || currency.Trim() == "")
        {
            throw new ArgumentException("Currency is required.");
        }

        if (method == null || method.Trim() == "")
        {
            throw new ArgumentException("Payment method is required.");
        }

        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative.");
        }

        decimal fee = 0m;
        string m = method.ToLower().Trim();

        if (m == "card")
        {
            fee = Math.Round((amount * 0.029m) + 0.30m, 2);
        }
        else if (m == "bank_transfer")
        {
            decimal raw = amount * 0.01m;
            if (raw > 5m)
            {
                raw = 5m;
            }

            fee = Math.Round(raw, 2);
        }
        else if (m == "wallet")
        {
            fee = Math.Round(amount * 0.015m, 2);
        }
        else
        {
            throw new ArgumentException("Unknown payment method: " + method);
        }

        if (isSubscription)
        {
            if (accountAgeDays > 365)
            {
                fee = Math.Round(fee * 0.5m, 2);
            }
            else
            {
                if (accountAgeDays > 90)
                {
                    fee = Math.Round(fee * 0.75m, 2);
                }
                else
                {
                    fee = Math.Round(fee * 0.9m, 2);
                }
            }
        }

        if (promoCode != null && promoCode.ToLower() == "waivefee")
        {
            fee = 0m;
        }

        decimal surcharge = 0m;
        DateTime now = NowProvider();
        if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
        {
            surcharge = Math.Round(amount * 0.005m, 2);
        }

        string riskBand;
        int score = 0;

        if (amount > 500m)
        {
            score = score + 2;
        }
        else
        {
            if (amount > 100m)
            {
                score = score + 1;
            }
        }

        if (country == null || country.ToLower() != "us")
        {
            score = score + 1;

            if (country != null && country.ToLower() == "ng")
            {
                score = score + 2;
            }
        }

        if (customerId.ToLower().StartsWith("new_"))
        {
            score = score + 1;
        }

        if (accountAgeDays < 30)
        {
            score = score + 1;
        }

        if (score >= 5)
        {
            riskBand = "CRITICAL";
        }
        else
        {
            if (score >= 3)
            {
                riskBand = "HIGH";
            }
            else
            {
                if (score == 2)
                {
                    riskBand = "MEDIUM";
                }
                else
                {
                    riskBand = "LOW";
                }
            }
        }

        string cur = currency.ToUpper().Trim();
        string cacheKey = cur + "|" + amount;
        decimal rate;

        if (RateCache.ContainsKey(cacheKey))
        {
            rate = RateCache[cacheKey];
        }
        else
        {
            try
            {
                RateLookupCount++;
                rate = _rateApi.GetUsdRate(cur);
                RateCache[cacheKey] = rate;
            }
            catch
            {
                rate = 1m;
            }
        }

        decimal total = Math.Round((amount + fee + surcharge) * rate, 2, MidpointRounding.AwayFromZero);

        LegacyReceipt receipt = new LegacyReceipt();
        receipt.Amount = amount;
        receipt.Fee = fee;
        receipt.Surcharge = surcharge;
        receipt.TotalUsd = total;
        receipt.RiskBand = riskBand;
        receipt.Currency = cur;

        return receipt;
    }

    /// <summary>
    /// Used by the checkout page to show the fee before the customer commits.
    /// </summary>
    public decimal EstimateFee(decimal amount, string method)
    {
        string m = method.ToLower().Trim();

        if (m == "card")
        {
            return Math.Round((amount * 0.029m) + 0.30m, 2, MidpointRounding.AwayFromZero);
        }

        if (m == "bank_transfer")
        {
            decimal f = Math.Round(amount * 0.01m, 2, MidpointRounding.AwayFromZero);
            if (f > 5m)
            {
                f = 5m;
            }

            return f;
        }

        if (m == "wallet")
        {
            return Math.Round(amount * 0.015m, 2, MidpointRounding.AwayFromZero);
        }

        throw new ArgumentException("Unknown payment method: " + method);
    }

    public List<LegacyReceipt> ProcessBatch(List<object[]> rows)
    {
        List<LegacyReceipt> results = new List<LegacyReceipt>();

        for (int i = 0; i < rows.Count; i++)
        {
            object[] r = rows[i];
            results.Add(ProcessCharge(
                (decimal)r[0],
                (string)r[1],
                (string)r[2],
                (string)r[3],
                (string)r[4],
                (bool)r[5],
                (int)r[6],
                (string)r[7]));
        }

        return results;
    }
}
