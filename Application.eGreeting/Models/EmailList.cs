using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.eGreeting.Models
{
    public class EmailList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string ListEmail { get; set; }
    }
}