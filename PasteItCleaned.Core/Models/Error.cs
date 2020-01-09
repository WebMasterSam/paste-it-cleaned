using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PasteItCleaned.Core.Models
{
    public class Error
    {
        [Column("error_id", TypeName = "char(36)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ErrorId { get; set; }

        [Column("client_id", TypeName = "char(36)")]
        public Guid ClientId { get; set; }

        [Column("error_message")]
        public string Message { get; set; }

        [Column("stack_trace")]
        public string StackTrace { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}
