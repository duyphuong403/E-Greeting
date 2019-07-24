using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.eGreeting.Models
{
    public class PaymentInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [RegularExpression("[0-9]{12,14}", ErrorMessage = "Account Number just can be from 12 - 14 digits")]
        public int BankAccount { get; set; }

        [Required]
        [Display(Name = "Expired")]
        [DataType(DataType.Date)]
        public DateTime DateExpire { get; set; }




    }
}