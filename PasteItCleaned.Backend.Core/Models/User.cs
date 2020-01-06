using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class User
    {
        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("cognito_id")]
        public string CognitoId { get; set; }

        [Column("cognito_username")]
        public string CognitoUsername { get; set; }


        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
