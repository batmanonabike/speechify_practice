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
