using System;

namespace PasteItCleaned.Backend.Entities
{
    public class UserEntity
    {
        public Guid UserId { get; set; }
        public string CognitoId { get; set; }
        public string CognitoUsername { get; set; }
    }
}
