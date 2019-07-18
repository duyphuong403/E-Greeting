using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.eGreeting.Models
{
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "NameCard cannot more than 50 characters. ")]
        public string NameCard { get; set; }

        [Required]        
        public string Category { get; set; }

        public string ImageName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateCreated { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime? DateCreated
        //{
        //    get
        //    {
        //        return this.dateCreated.HasValue
        //           ? this.dateCreated.Value
        //           : DateTime.Now;
        //    }

        //    set { this.dateCreated = value; }
        //}

        //private DateTime? dateCreated = null;
    }
}