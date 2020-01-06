using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Domain
    {
        [Column("domain_id")]
        public Guid DomainId { get; set; }

        [Column("domain_name")]
        public string Name { get; set; }

        [Column("api_key_id")]
        public Guid ApiKeyId { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
