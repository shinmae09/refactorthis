using RefactorThis.Persistence.Entities.Enums;
using RefactorThis.Persistence.Entities.Models;
using RefactorThis.Persistence.Models;

namespace RefactorThis.Domain.Factories
{
    public class CommercialInvoiceHandler : InvoiceHandler
    {
        private readonly decimal TAX_PERCENTAGE;
        public CommercialInvoiceHandler(Invoice invoice) : base(invoice)
        {
            // Assuming a fixed tax percentage for commercial invoices
            TAX_PERCENTAGE = 0.14m; // 14% tax
        }

        public override string HandleInvoiceAndPayment(Payment payment)
        {
            // Custom logic for handling commercial invoices can be added here
            if (_invoice.Type == InvoiceType.Commercial)
            {
                _invoice.TaxAmount += payment.Amount * TAX_PERCENTAGE;
            }

            return base.HandleInvoiceAndPayment(payment);
        }

        public override string HandleRefund(Refund refund)
        {
            return base.HandleRefund(refund);
        }
    }
}