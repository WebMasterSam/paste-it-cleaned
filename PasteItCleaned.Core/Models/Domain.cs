using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Core.Models
{
    public class Domain
    {
        [Column("domain_id", TypeName = "char(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid DomainId { get; set; }

        [Column("domain_name")]
        public string Name { get; set; }

        [Column("api_key_id", TypeName = "char(36)")]
        public Guid ApiKeyId { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime? UpdatedOn { get; set; }

        [Column("deleted", TypeName = "bit(1)")]
        public bool Deleted { get; set; }
    }
}
