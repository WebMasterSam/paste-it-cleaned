using System;

namespace PasteItCleaned.Backend.Entities
{
    public class PaymentEntity
    {
        public Guid PaymentId { get; set; }
        public string TrxNumber { get; set; }
        public decimal Total { get; set; }
        public PaymentMethodEntity PaymentMethod { get; set; }
    }
}
