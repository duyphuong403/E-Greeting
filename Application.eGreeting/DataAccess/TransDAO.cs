using Application.eGreeting.Models;
using System.Collections.Generic;

namespace Application.eGreeting.DataAccess
{
    public class TransDAO
    {
        private static eGreetingDB db = new eGreetingDB();

        public static IEnumerable<Transaction> GetAllTrans { get => db.Transactions; }

        public static Transaction GetTransaction(int id)
        {
            return db.Transactions.Find(id);
        }

        public static bool CreateTrans(Transaction newTrans)
        {
            var b = GetTransaction(newTrans.TransId);
            if (b == null)
            {
                db.Transactions.Add(newTrans);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool DeleteTrans(int id)
        {
            var b = GetTransaction(id);
            if (b != null)
            {
                db.Transactions.Remove(b);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}