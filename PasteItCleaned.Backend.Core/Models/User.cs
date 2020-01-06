using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public Guid ClientId { get; set; }
        public string CognitoId { get; set; }
        public string CognitoUsername { get; set; }

        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
    }
}
