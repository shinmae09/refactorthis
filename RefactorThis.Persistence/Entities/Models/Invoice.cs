using RefactorThis.Persistence.Entities.Enums;
using RefactorThis.Persistence.Entities.Models;
using System;
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
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<Refund> Refunds { get; set; } = new List<Refund>();
        public InvoiceType Type { get; set; }
        public DateTime DueDate { get; set; }

        public decimal GetAmountDue()
        {
            return this.Amount - this.Payments.Sum(p => p.Amount);
        }

        public bool HasPayments()
        {
            return this.Payments != null && this.Payments.Any();
        }

        public bool IsOverdue()
        {
            return this.DueDate < DateTime.UtcNow && this.GetAmountDue() > 0;
        }

        public int GetOverdueDays()
        {
            if (!this.IsOverdue())
            {
                return 0;
            }
            var overdueDays = (DateTime.UtcNow - this.DueDate).Days;
            return overdueDays < 0 ? 0 : overdueDays;
        }

        public decimal CalculatePenalty(decimal penaltyRate)
        {
            if (!this.IsOverdue())
            {
                return 0;
            }

            return penaltyRate * this.GetOverdueDays();
        }
    }
}