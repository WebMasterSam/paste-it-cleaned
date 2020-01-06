using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Config
    {
        [Column("config_id")]
        public Guid ConfigId { get; set; }

        [Column("config_name")]
        public string Name { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("embed_external_images")]
        public bool EmbedExternalImages { get; set; }

        [Column("remove_empty_tags")]
        public bool RemoveEmptyTags { get; set; }

        [Column("remove_span_tags")]
        public bool RemoveSpanTags { get; set; }

        [Column("remove_class_names")]
        public bool RemoveClassNames { get; set; }

        [Column("remove_iframes")]
        public bool RemoveIframes { get; set; }

        [Column("remove_tag_attributes")]
        public bool RemoveTagAttributes { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
