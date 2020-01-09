using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Core.Models
{
    public class PaymentMethod
    {
        [Column("payment_method_id", TypeName = "char(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid PaymentMethodId { get; set; }

        [Column("client_id", TypeName = "char(36)")]
        public Guid ClientId { get; set; }

        [Column("payment_method_type")]
        public string Type { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime? UpdatedOn { get; set; }

        [Column("deleted", TypeName = "bit(1)")]
        public bool Deleted { get; set; }
    }
}
