namespace RefactorThis.Domain.Constants
{
    public class ReturnMessage
    {
        public const string FINAL_PARTIAL_PAYMENT_MESSAGE = "Final partial payment received, invoice is now fully paid.";
        public const string INVOICE_ALREADY_FULLY_PAID_MESSAGE = "Invoice was already fully paid.";
        public const string INVOICE_NOW_FULLY_PAID_MESSAGE = "Invoice is now fully paid.";
        public const string INVOICE_NOW_PARTIALLY_PAID_MESSAGE = "Invoice is now partially paid.";
        public const string NEW_PARTIAL_PAYMENT_BUT_NOT_FULLY_PAID_MESSAGE = "Another partial payment received, still not fully paid.";
        public const string NO_PAYMENT_NEEDED_MESSAGE = "No payment needed.";
        public const string PAYMENT_GREATER_THAN_INVOICE_AMOUNT_MESSAGE = "The payment is greater than the invoice amount.";
        public const string PAYMENT_GREATER_THAN_PARTIAL_AMOUNT_MESSAGE = "The payment is greater than the partial amount remaining.";
    }
}