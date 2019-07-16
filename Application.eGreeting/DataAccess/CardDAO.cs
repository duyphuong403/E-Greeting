using Application.eGreeting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.eGreeting.DataAccess
{
    public class CardDAO
    {
        private static eGreetingDB db = new eGreetingDB();

        public static IEnumerable<Card> GetAllCard { get => db.Cards; }

        public static Card GetCard(int id)
        {
            return db.Cards.Find(id);
        }

        public static bool Create(Card newCard)
        {
            var b = GetCard(newCard.CardId);
            if (b == null)
            {
                db.Cards.Add(newCard);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool EditCard(Card Card)
        {
            var b = GetCard(Card.CardId);
            if (b != null)
            {
                b.Category = Card.Category;
                b.ImageName = Card.ImageName;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool DeleteCard(int id)
        {
            var b = GetCard(id);
            if (b != null)
            {
                db.Cards.Remove(b);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}