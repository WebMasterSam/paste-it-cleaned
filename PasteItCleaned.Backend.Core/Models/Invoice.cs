using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public Guid ClientId { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
