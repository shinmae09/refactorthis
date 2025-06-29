namespace RefactorThis.Domain.Constants
{
    public class ValidationMessage
    {
        public const string INVALID_INVOICE_STATE_MESSAGE = "The invoice is in an invalid state, it has an amount of 0 and it has payments.";
        public const string INVALID_INVOICE_TYPE_MESSAGE = "Invalid invoice type";
        public const string NO_INVOICE_FOUND_MESSAGE = "There is no invoice matching this payment.";
    }
}