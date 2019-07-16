using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.eGreeting.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name ="Role")]
        public bool Role { get; set; }

        [Required]
        [Display(Name = "Is Subcribe Send")]
        public bool IsSubcribeSend { get; set; }

        [Required]
        [Display(Name = "Is Subcribe Receive")]
        public bool IsSubcribeReceive { get; set; }

        [Required]
        [StringLength(50, MinimumLength =6)]
        [Display(Name = "Full Name")]
        public int FullName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public bool Gender { get; set; }

        [Required]
        [MaxLength(10),MinLength(8)]
        [Display(Name ="Phone Number")]
        public int Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength =10)]
        [Display(Name ="Email Address")]
        public string Email { get; set; }
    }
}