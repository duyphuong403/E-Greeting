using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Application.eGreeting.Models;

namespace Application.eGreeting.DataAccess
{
    public class FeedbackDAO
    {
        private static eGreetingDB db = new eGreetingDB();

        public static IEnumerable<Feedback> GetAllFeedback { get => db.Feedbacks; }

        public Feedback GetById(int id)
        {
            return db.Feedbacks.Find(id);
        }

        public static bool Insert(Feedback newFeedback)
        {
            try
            {
                db.Feedbacks.Add(newFeedback);
                db.SaveChanges();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        public bool Delete(int id)
        {
            var b = GetById(id);
            if (b != null)
            {
                db.Feedbacks.Remove(b);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}