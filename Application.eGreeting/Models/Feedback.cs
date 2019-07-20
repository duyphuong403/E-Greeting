using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.eGreeting.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        // title
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "DataCreated")]
        public DateTime DataCreated { get; set; }
    }
}