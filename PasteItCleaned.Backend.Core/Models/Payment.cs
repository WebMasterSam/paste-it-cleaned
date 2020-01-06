using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string TrxNumber { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
