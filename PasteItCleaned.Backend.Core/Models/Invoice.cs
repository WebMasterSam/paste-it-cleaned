using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Invoice
    {
        [Column("invoice_id", TypeName = "char(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid InvoiceId { get; set; }

        [Column("client_id", TypeName = "char(36)")]
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

        [Column("paid", TypeName = "bit(1)")]
        public bool Paid { get; set; }

        [Column("paid_on")]
        public DateTime? PaidOn { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
