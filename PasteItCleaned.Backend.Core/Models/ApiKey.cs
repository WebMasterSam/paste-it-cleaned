using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class ApiKey
    {
        [Column("api_key_id", TypeName = "char(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ApiKeyId { get; set; }

        [Column("content")]
        public string Key { get; set; }

        [Column("client_id", TypeName = "char(36)")]
        public Guid ClientId { get; set; }

        [Column("expires_on")]
        public DateTime ExpiresOn { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime? UpdatedOn { get; set; }

        [Column("deleted", TypeName = "bit(1)")]
        public bool Deleted { get; set; }
    }
}
