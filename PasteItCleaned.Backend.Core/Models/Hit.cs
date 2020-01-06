using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Hit
    {
        [Column("hit_id")]
        public Guid HitId { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("hit_date")]
        public DateTime Date { get; set; }

        [Column("hit_hash")]
        public int Hash { get; set; }

        [Column("ip")]
        public string Ip { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("hit_type")]
        public string Type { get; set; }

        [Column("referer")]
        public string Referer { get; set; }
    }
}
