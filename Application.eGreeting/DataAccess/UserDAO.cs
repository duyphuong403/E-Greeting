using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Application.eGreeting.Models;


namespace Application.eGreeting.DataAccess
{
    public class UserDAO
    {
        private static eGreetingDB db = new eGreetingDB();

        public static IEnumerable<User> GetAllUser { get => db.Users; }

        public static User CheckLogin(User user)
        {
            return db.Users.FirstOrDefault(item => item.UserName == user.UserName && item.Password == user.Password);
        }

        public static User GetUser(int id)
        {
            return db.Users.Find(id);
        }

        public static bool Create(User newUser)
        {
            var b = GetUser(newUser.UserId);
            if (b == null)
            {
                db.Users.Add(newUser);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool EditUser(User editUser)
        {
            var b = GetUser(editUser.UserId);
            if (b != null)
            {
                b.Password = editUser.Password;
                b.IsSubcribeSend = editUser.IsSubcribeSend;
                b.IsSubcribeReceive = editUser.IsSubcribeReceive;
                b.FullName = editUser.FullName;
                b.Gender = editUser.Gender;
                b.Phone = editUser.Phone;
                b.Email = editUser.Email;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool DeleteUser(int id)
        {
            var b = GetUser(id);
            if (b != null)
            {
                db.Users.Remove(b);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}