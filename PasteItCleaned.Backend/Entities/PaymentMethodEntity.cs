using System;

namespace PasteItCleaned.Backend.Entities
{
    public class PaymentMethodEntity
    {
        public Guid PaymentMethodId { get; set; }
        public string Type { get; set; }
        public bool PaymentFailed { get; set; }
    }
}
