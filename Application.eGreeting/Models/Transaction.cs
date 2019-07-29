using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.eGreeting.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransId { get; set; }

        [Required]
        [Display(Name ="Sender")]
        [StringLength(50, ErrorMessage = "Sender cannot more than 50 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage ="Receiver cannot more than 50 characters")]
        public string Receiver { get; set; }      
     
        [Required]
        [StringLength(100, ErrorMessage ="Subject cannot more than 100 characters.")]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage ="Content cannot more than 1000 characters.")]
        public string Content { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "NameCard cannot more than 50 characters")]
        public string NameCard { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "ImageName cannot more than 50 characters")]
        public string ImageNameTrans { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TimeSend { get; set; }
    }
}