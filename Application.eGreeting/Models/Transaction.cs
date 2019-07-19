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

        //foreign key with Users
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        [StringLength(50, ErrorMessage ="Receiver cannot more than 50 characters")]
        public string Receiver { get; set; }

        // foreign key with Cards
        public int CardId { get; set; }
        public Card Card { get; set; }

        [Required]
        [StringLength(100, ErrorMessage ="Subject cannot more than 100 characters.")]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage ="Content cannot more than 1000 characters.")]
        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TimeSend { get; set; }
    }
}