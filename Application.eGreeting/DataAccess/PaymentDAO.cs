using Application.eGreeting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.eGreeting.DataAccess
{
    public class PaymentDAO
    {
        private static readonly eGreetingDB db = new eGreetingDB();

        public static List<PaymentInfo> GetAllPayment { get => db.PaymentInfos.ToList(); }


        public static PaymentInfo GetPayment(int id)
        {
            return db.PaymentInfos.Find(id);
        }

        public static PaymentInfo GetPaymentByUsername(string username)
        {
            return db.PaymentInfos.FirstOrDefault(item => item.UserName == username);
        }

        public static bool CreatePayment(PaymentInfo newUser)
        {
            var b = GetPayment(newUser.PayId);
            if (b == null)
            {
                db.PaymentInfos.Add(newUser);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool EditPayment(PaymentInfo editPayment)
        {
            var b = GetPayment(editPayment.PayId);
            if (b != null)
            {
                b.BankAccount = editPayment.BankAccount;
                b.BankName = editPayment.BankName;
                b.DateExpire = editPayment.DateExpire;
                b.IsActive = editPayment.IsActive;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool EditPaymentInfo(PaymentInfo editPayment)
        {
            var b = GetPayment(editPayment.PayId);
            if (b != null)
            {
                b.BankAccount = editPayment.BankAccount;
                b.BankName = editPayment.BankName;
                b.DateExpire = editPayment.DateExpire;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool DeletePayment(int id)
        {
            var b = GetPayment(id);
            if (b != null)
            {
                db.PaymentInfos.Remove(b);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool ChangeStatusActivation(int id, bool check)
        {
            var search = GetPayment(id);
            if (search != null)
            {
                if (!check)
                {
                    search.IsActive = check;
                }
                else
                {
                    search.IsActive = check;
                    search.DateCreated = DateTime.Now;
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}