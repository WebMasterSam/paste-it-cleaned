using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class PaymentMethod
    {
        [Column("payment_method_id")]
        public Guid PaymentMethodId { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("payment_method_type")]
        public string Type { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
