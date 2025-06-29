using RefactorThis.Domain.Constants;
using RefactorThis.Persistence.Extensions;
using RefactorThis.Persistence.Models;

namespace RefactorThis.Domain.Factories
{
    public abstract class InvoiceHandler
    {
        protected Invoice _invoice;
        protected Payment _payment;

        public InvoiceHandler(Invoice invoice, Payment payment)
        {
            _invoice = invoice.ThrowIfNull(nameof(invoice));
            _payment = payment.ThrowIfNull(nameof(payment));
        }

        public virtual string HandleInvoiceAndPayment()
        {
            _invoice.Payments.Add(_payment);

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