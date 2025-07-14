using System;

namespace RefactorThis.Persistence.Entities.Models
{
    public class Refund
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
