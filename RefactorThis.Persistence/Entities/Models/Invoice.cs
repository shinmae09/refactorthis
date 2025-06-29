using RefactorThis.Persistence.Entities.Enums;
using System.Collections.Generic;
using System.Linq;

namespace RefactorThis.Persistence.Models
{
    public class Invoice
    {
        public long Id { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public List<Payment> Payments { get; set; }
        public InvoiceType Type { get; set; }

        public decimal GetAmountDue()
        {
            return this.Amount - this.Payments.Sum(p => p.Amount);
        }

        public bool HasPayments()
        {
            return this.Payments != null && this.Payments.Any();
        }
    }
}