using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.eGreeting.Models
{
    public class SlideCard
    {
        public IList<Card> ListBirthday { get; set; }

        public IList<Card> ListNewYear { get; set; }

        public IList<Card> ListFestival { get; set; }
    }
}