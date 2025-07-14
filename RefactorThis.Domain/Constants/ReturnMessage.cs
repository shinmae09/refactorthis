namespace RefactorThis.Domain.Constants
{
    public class ReturnMessage
    {
        #region PAYMENT RETURN MESSAGES
        public const string FINAL_PARTIAL_PAYMENT_MESSAGE = "Final partial payment received, invoice is now fully paid.";
        public const string INVOICE_ALREADY_FULLY_PAID_MESSAGE = "Invoice was already fully paid.";
        public const string INVOICE_NOW_FULLY_PAID_MESSAGE = "Invoice is now fully paid.";
        public const string INVOICE_NOW_PARTIALLY_PAID_MESSAGE = "Invoice is now partially paid.";
        public const string NEW_PARTIAL_PAYMENT_BUT_NOT_FULLY_PAID_MESSAGE = "Another partial payment received, still not fully paid.";
        public const string NO_PAYMENT_NEEDED_MESSAGE = "No payment needed.";
        public const string PAYMENT_GREATER_THAN_INVOICE_AMOUNT_MESSAGE = "The payment is greater than the invoice amount.";
        public const string PAYMENT_GREATER_THAN_PARTIAL_AMOUNT_MESSAGE = "The payment is greater than the partial amount remaining.";
        #endregion

        #region REFUND RETURN MESSAGES
        public const string REFUND_PROCESSED_SUCCESSFULLY_MESSAGE = "Refund processed successfully.";
        public const string REFUND_AMOUNT_EXCEEDS_TOTAL_PAID_MESSAGE = "Refund amount exceeds total amount paid.";
        public const string REFUND_CANNOT_BE_PROCESSED_NO_PAYMENTS_MADE_ON_INVOICE_MESSAGE = "No payments have been made on this invoice, refund cannot be processed.";
        public const string REFUNDS_ONLY_FOR_COMMERCIAL_INVOICES_MESSAGE = "Refunds are only applicable for commercial invoices.";
        #endregion
    }
}