using RefactorThis.Persistence.Models;

namespace RefactorThis.Domain.Factories
{
    public class StandardInvoiceHandler : InvoiceHandler
    {
        public StandardInvoiceHandler(Invoice invoice) : base(invoice)
        {
        }

        public override string HandleInvoiceAndPayment(Payment payment)
        {
            // Custom logic for handling standard invoices can be added here
            return base.HandleInvoiceAndPayment(payment);
        }
    }
}