using RefactorThis.Domain.Constants;
using RefactorThis.Persistence.Entities.Models;
using RefactorThis.Persistence.Extensions;
using RefactorThis.Persistence.Models;
using System.Linq;

namespace RefactorThis.Domain.Factories
{
    public abstract class InvoiceHandler
    {
        protected Invoice _invoice;

        public InvoiceHandler(Invoice invoice)
        {
            _invoice = invoice.ThrowIfNull(nameof(invoice));
        }

        public virtual string HandleRefund(Refund refund)
        {
            if (_invoice.HasPayments())
            {
                var totalAmountPaid = _invoice.Payments.Sum(p => p.Amount);
                if (refund.Amount > totalAmountPaid)
                {
                    return ReturnMessage.REFUND_AMOUNT_EXCEEDS_TOTAL_PAID_MESSAGE;
                }

                _invoice.Refunds.Add(refund);
            }
            else
            {
                return ReturnMessage.REFUND_CANNOT_BE_PROCESSED_NO_PAYMENTS_MADE_ON_INVOICE_MESSAGE;
            }

            return ReturnMessage.REFUND_PROCESSED_SUCCESSFULLY_MESSAGE;
        }

        public virtual string HandleInvoiceAndPayment(Payment payment)
        {
            _invoice.Payments.Add(payment);

            if (_invoice.GetAmountDue() <= 0)
            {
                if (_invoice.Payments.Count == 1)
                {
                    return ReturnMessage.INVOICE_NOW_FULLY_PAID_MESSAGE;
                }
                else
                {
                    return ReturnMessage.FINAL_PARTIAL_PAYMENT_MESSAGE;
                }
            }
            else
            {
                if (_invoice.Payments.Count == 1)
                {
                    return ReturnMessage.INVOICE_NOW_PARTIALLY_PAID_MESSAGE;
                }
                else
                {
                    return ReturnMessage.NEW_PARTIAL_PAYMENT_BUT_NOT_FULLY_PAID_MESSAGE;
                }
            }
        }
    }
}