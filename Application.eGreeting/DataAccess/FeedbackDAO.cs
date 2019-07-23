using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Application.eGreeting.Models;
using PagedList; // Pagination

namespace Application.eGreeting.DataAccess
{
    public class FeedbackDAO
    {
        private static eGreetingDB db = new eGreetingDB();

        //public static IEnumerable<Feedback> GetAllFeedback { get => db.Feedbacks; }

        public static IList<FeedbackModel> GetList(Pagination pagination) {
            string query = string.Format(
                "with tbl as "+
                    "(select ROW_NUMBER() over(order by Id desc) as myIndex, "+
                    "count(Id) over() as total, * "+
                    "from Feedbacks) "+
                "select* from tbl where myIndex between {0} and {1}", pagination.from, pagination.to );
            return db.Database.SqlQuery<FeedbackModel>(query).ToList();
        }

        public static IEnumerable<Feedback> GetAllFeedbackPaging(int page, int pageSize)
        {
            return db.Feedbacks.OrderByDescending(o => o.Id).ToPagedList(page, pageSize);
        }

        public static Feedback GetById(int id)
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

        public static bool Update(Feedback feedback)
        {
            try
            {
                db.Feedbacks.AddOrUpdate(feedback);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }

        public static bool Delete(int id)
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