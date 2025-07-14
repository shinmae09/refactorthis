using RefactorThis.Persistence.Entities.Enums;
using RefactorThis.Persistence.Entities.Models;
using RefactorThis.Persistence.Models;

namespace RefactorThis.Domain.Factories
{
    public class CommercialInvoiceHandler : InvoiceHandler
    {
        private readonly decimal TAX_PERCENTAGE;
        private readonly decimal PENALTY_RATE; 

        public CommercialInvoiceHandler(Invoice invoice) : base(invoice)
        {
            // Assuming a fixed tax percentage for commercial invoices
            TAX_PERCENTAGE = 0.14m; // 14% tax

            // Assuming a fixed penalty rate for commercial invoices
            PENALTY_RATE = 0.05m;
        }

        public override string HandleInvoiceAndPayment(Payment payment)
        {
            // Custom logic for handling commercial invoices can be added here
            _invoice.TaxAmount += payment.Amount * TAX_PERCENTAGE;

            if (_invoice.IsOverdue()) 
            { 
                var totalDue = _invoice.GetAmountDue();
                var penalty = _invoice.CalculatePenalty(PENALTY_RATE);

                totalDue += penalty;
            }

            return base.HandleInvoiceAndPayment(payment);
        }

        public override string HandleRefund(Refund refund)
        {
            return base.HandleRefund(refund);
        }
    }
}