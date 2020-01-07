using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Payment
    {
        [Column("payment_id", TypeName = "char(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid PaymentId { get; set; }

        [Column("invoice_id", TypeName = "char(36)")]
        public Guid InvoiceId { get; set; }

        [Column("payment_method_id", TypeName = "char(36)")]
        public Guid PaymentMethodId { get; set; }

        [Column("trx_number")]
        public string TrxNumber { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
