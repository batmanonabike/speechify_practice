namespace SpeechifyPractice.Refactor;

public sealed class PaymentFeeCalculator : IFeeCalculator
{
    public decimal Calculate(decimal amount, PaymentMethod method)
    {
        // Practice task: extract + simplify the legacy fee rules here.
        throw new NotImplementedException("Implement fee rules for Card, BankTransfer, and Wallet.");
    }
}
