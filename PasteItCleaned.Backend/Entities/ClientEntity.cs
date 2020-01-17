using System;

namespace PasteItCleaned.Backend.Entities
{
    public class ClientEntity
    {
        public Guid ClientId { get; set; }
        public decimal Balance { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string TimeZone { get; set; }
    }
}
