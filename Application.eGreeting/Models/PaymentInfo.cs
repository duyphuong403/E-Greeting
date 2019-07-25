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

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        [StringLength(50, ErrorMessage = "Bank Name can't more than 50 characters")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [RegularExpression("[0-9]{12,16}", ErrorMessage = "Account Number just can be from 12 - 16 digits")]
        public long BankAccount { get; set; }

        [Required]
        [Display(Name = "Expired")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateExpire { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Status Activation")]
        public bool IsActive { get; set; }




    }
}