using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class PaymentMethod
    {
        public Guid PaymentMethodId { get; set; }
        public Guid ClientId { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
