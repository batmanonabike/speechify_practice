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
