using RefactorThis.Persistence.Models;

namespace RefactorThis.Domain.Factories
{
    public class StandardInvoiceHandler : InvoiceHandler
    {

        private readonly decimal PENALTY_RATE;  

        public StandardInvoiceHandler(Invoice invoice) : base(invoice)
        {
            PENALTY_RATE = 0.05m;
        }

        public override string HandleInvoiceAndPayment(Payment payment)
        {
            if (_invoice.IsOverdue())
            {
                var totalDue = _invoice.GetAmountDue();
                var penalty = _invoice.CalculatePenalty(PENALTY_RATE);

                totalDue += penalty;
            }

            // Custom logic for handling standard invoices can be added here
            return base.HandleInvoiceAndPayment(payment);
        }
    }
}