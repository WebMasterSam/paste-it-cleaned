using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Client
    {
        [Column("client_id")]
        public Guid ClientId { get; set; }


        [Column("balance")]
        public decimal Balance { get; set; }


        [Column("business_name")]
        public string BusinessName { get; set; }


        [Column("address")]
        public string Address { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("country_code")]
        public string Country { get; set; }

        [Column("state_code")]
        public string State { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }


        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [Column("deleted")]
        public bool Deleted { get; set; }
    }
}
