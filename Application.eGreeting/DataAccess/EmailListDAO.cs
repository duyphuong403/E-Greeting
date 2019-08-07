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

        public static EmailList GetEmailByUsername(string username)
        {
            return db.EmailLists.FirstOrDefault(item => item.Username == username);
        }
        public static bool Create(EmailList emailList)
        {
            db.EmailLists.Add(emailList);
            db.SaveChanges();
            return true;
        }
    }
}