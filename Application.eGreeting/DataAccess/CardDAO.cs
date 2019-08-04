using Application.eGreeting.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.eGreeting.DataAccess
{
    public class CardDAO
    {
        private static readonly eGreetingDB db = new eGreetingDB();

        public static List<Card> GetAllCard { get => db.Cards.OrderByDescending(o => o.DateCreated).ToList(); }

        public static Card GetCard(int id)
        {
            return db.Cards.Find(id);
        }

        public static bool GetNameCard(string nameCard)
        {
            return db.Cards.Any(o => o.NameCard == nameCard);
        }

        public static bool GetImageCard(string ImageName)
        {
            return db.Cards.Any(o => o.ImageName == ImageName);
        }

        public static List<Card> GetCards(string name)
        {
            name = name.ToLower();
            return db.Cards.Where(item => item.NameCard.ToLower().Contains(name)).ToList();
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
                if (Card.ImageName != null)
                {
                    b.NameCard = Card.NameCard;
                    b.Category = Card.Category;
                    b.ImageName = Card.ImageName;
                    b.DateCreated = Card.DateCreated;
                    db.SaveChanges();
                }
                else
                {
                    b.NameCard = Card.NameCard;
                    b.Category = Card.Category;
                    b.DateCreated = Card.DateCreated;
                    db.SaveChanges();
                }
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
        public static IEnumerable<SelectListItem> GetCategoryList()
        {
            var model = new List<SelectListItem>()
            {
                new SelectListItem{ Value="", Text="---Select Category---", Selected=true},
                new SelectListItem{ Value="birthday", Text="Birthday"},
                new SelectListItem{ Value="newyear", Text="NewYear"},
                new SelectListItem{ Value="festival", Text="Festival"},
            };
            return model;
        }
        public static List<Card> GetCardByCategory(string Category)
        {
            return db.Cards.Where(item => item.Category.ToLower().Contains(Category.ToLower())).ToList();
        }

    }
}

