using Application.eGreeting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.eGreeting.DataAccess
{
    public class EmailListDAO
    {
        private static readonly eGreetingDB db = new eGreetingDB();

        public static EmailList GetEmailList(int id) { return db.EmailLists.Find(id); }

        public static EmailList GetEmailListByUsername(string username) { return db.EmailLists.FirstOrDefault(o => o.Username == username); }
        public static List<EmailList> SearchEmailListByUsername(string username)
        {
            return db.EmailLists.Where(item => item.Username == username).ToList();
        }
        public static bool Create(EmailList emailList)
        {
            db.EmailLists.Add(emailList);
            db.SaveChanges();
            return true;
        }

        public static bool Edit(EmailList editEmailList)
        {
            var search = GetEmailList(editEmailList.EmailId);
            if (search != null)
            {
                search.ListEmail = editEmailList.ListEmail;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool Delete(int id)
        {
            var b = GetEmailList(id);
            if (b != null)
            {
                db.EmailLists.Remove(b);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}