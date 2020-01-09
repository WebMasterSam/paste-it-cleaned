using System;

namespace PasteItCleaned.Backend.Entities
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidOn { get; set; }
        public Payment Payment { get; set; }
    }
}
