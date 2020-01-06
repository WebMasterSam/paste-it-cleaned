using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Invoice
    {
        [Column("invoice_id")]
        public Guid InvoiceId { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("invoice_number")]
        public int Number { get; set; }

        [Column("invoice_date")]
        public DateTime Date { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("taxes")]
        public decimal Taxes { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("paid")]
        public bool Paid { get; set; }

        [Column("paid_on")]
        public DateTime PaidOn { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
