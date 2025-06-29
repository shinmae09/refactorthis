using RefactorThis.Persistence.Models;

namespace RefactorThis.Domain.Factories
{
    public class StandardInvoiceHandler : InvoiceHandler
    {
        public StandardInvoiceHandler(Invoice invoice, Payment payment) : base(invoice, payment)
        {
        }

        public override string HandleInvoiceAndPayment()
        {
            // Custom logic for handling standard invoices can be added here
            return base.HandleInvoiceAndPayment();
        }
    }
}