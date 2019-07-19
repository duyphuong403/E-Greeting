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
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "User Name")]
        [RegularExpression(@"[^\s]+", ErrorMessage = "Cannot use special character!")]        
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        [Display(Name = "RePassword")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

        [Display(Name ="Role")]
        public bool Role { get; set; }

        [Display(Name = "Is Subcribe Send")]
        public bool IsSubcribeSend { get; set; }

        [Display(Name = "Is Subcribe Receive")]
        public bool IsSubcribeReceive { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public bool Gender { get; set; }

        [Required]
        [RegularExpression("[0-9]{8,10}", ErrorMessage ="Phone Number must be from 8 - 10 digits")]
        [Display(Name ="Phone Number")]
        public int Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength =10)]
        [Display(Name ="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}