using RefactorThis.Persistence.Entities.Enums;
using RefactorThis.Persistence.Models;

namespace RefactorThis.Domain.Factories
{
    public class CommercialInvoiceHandler : InvoiceHandler
    {
        private readonly decimal TAX_PERCENTAGE;
        public CommercialInvoiceHandler(Invoice invoice, Payment payment) : base(invoice, payment)
        {
            // Assuming a fixed tax percentage for commercial invoices
            TAX_PERCENTAGE = 0.14m; // 14% tax
        }

        public override string HandleInvoiceAndPayment()
        {
            // Custom logic for handling commercial invoices can be added here
            if (_invoice.Type == InvoiceType.Commercial)
            {
                _invoice.TaxAmount += _payment.Amount * TAX_PERCENTAGE;
            }

            return base.HandleInvoiceAndPayment();
        }
    }
}