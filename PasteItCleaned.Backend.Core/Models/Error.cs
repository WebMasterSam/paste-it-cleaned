using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Backend.Core.Models
{
    public class Error
    {
        [Column("error_id")]
        public Guid ErrorId { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("error_message")]
        public string Message { get; set; }

        [Column("stack_trace")]
        public string StackTrace { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
