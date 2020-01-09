using System;

namespace PasteItCleaned.Backend.Entities
{
    public class PaymentMethod
    {
        public Guid PaymentMethodId { get; set; }
        public string Type { get; set; }
    }
}
