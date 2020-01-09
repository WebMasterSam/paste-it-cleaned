using System;

namespace PasteItCleaned.Backend.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string TrxNumber { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
