using System;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Client
    {
        public Guid ClientId { get; set; }
        //public List<Guid> ApiKeys { get; set; }
        //public List<Guid> Configs { get; set; }
        public string Key { get; set; }

        public decimal Balance { get; set; }

        public string BusinessName { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Sate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
    }
}
