using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Core.Models
{
    public class Config
    {
        [Column("config_id", TypeName = "char(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ConfigId { get; set; }

        [Column("config_name")]
        public string Name { get; set; }

        [Column("client_id", TypeName = "char(36)")]
        public Guid ClientId { get; set; }

        [Column("embed_external_images", TypeName = "bit(1)")]
        public bool EmbedExternalImages { get; set; }

        [Column("remove_empty_tags", TypeName = "bit(1)")]
        public bool RemoveEmptyTags { get; set; }

        [Column("remove_span_tags", TypeName = "bit(1)")]
        public bool RemoveSpanTags { get; set; }

        [Column("remove_class_names", TypeName = "bit(1)")]
        public bool RemoveClassNames { get; set; }

        [Column("remove_iframes", TypeName = "bit(1)")]
        public bool RemoveIframes { get; set; }

        [Column("remove_tag_attributes", TypeName = "bit(1)")]
        public bool RemoveTagAttributes { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime? UpdatedOn { get; set; }

        [Column("deleted", TypeName = "bit(1)")]
        public bool Deleted { get; set; }
    }
}
