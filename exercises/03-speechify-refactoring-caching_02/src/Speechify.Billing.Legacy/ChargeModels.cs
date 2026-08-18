namespace Speechify.Billing.Legacy;

// Mutable bag with public setters. Nothing stops a caller mutating a receipt
// after it has been handed out.
public class LegacyReceipt
{
    public decimal Amount { get; set; }

    public decimal Fee { get; set; }

    public decimal Surcharge { get; set; }

    public decimal TotalUsd { get; set; }

    public string RiskBand { get; set; } = "";

    public string Currency { get; set; } = "";
}
