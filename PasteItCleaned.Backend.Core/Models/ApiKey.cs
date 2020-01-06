﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class ApiKey
    {
        [Column("api_key_id")]
        public Guid ApiKeyId { get; set; }

        [Column("content")]
        public string Key { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("expires_on")]
        public DateTime ExpiresOn { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
