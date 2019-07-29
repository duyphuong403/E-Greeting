namespace Application.eGreeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class eGreetingDB : DbContext
    {
        // Your context has been configured to use a 'eGreetingDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Application.eGreeting.Models.eGreetingDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'eGreetingDB' 
        // connection string in the application configuration file.
        public eGreetingDB()
            : base("name=eGreetingDB")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Card> Cards { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<Feedback> Feedbacks { get; set; }

        public virtual DbSet<PaymentInfo> PaymentInfos { get; set; }

    }

    public class DBInit : DropCreateDatabaseIfModelChanges<eGreetingDB>
    {
        protected override void Seed(eGreetingDB context)
        {
            // Seed some record for Payment
            var dsPayment = new List<PaymentInfo>
            {
                new PaymentInfo {UserId = 2, UserName = "test", BankName = "ACB", BankAccount=9405123465478545, DateExpire = DateTime.ParseExact("2022-01-12","yyyy-MM-dd",null), DateCreated = DateTime.Now}
            };
            dsPayment.ForEach(item => context.PaymentInfos.Add(item));

            // Seed some record for Feedback
            var dsFeedback = new List<Feedback>
            {
                new Feedback {Subject="test", Content = "Hello Handsome Guys", Username = "test", DataCreated = DateTime.Now}
            };
            dsFeedback.ForEach(item => context.Feedbacks.Add(item));

            // Seed some record for Cards
            var ds = new List<Card>
            {
                new Card { NameCard="eGreeting-Birthday-001",Category="Birthday",ImageName="image1.jpg"},
            };
            ds.ForEach(item => context.Cards.Add(item));

            // Seed for Trans
            var dsTrans = new List<Transaction>
            {
                new Transaction { Username = "test", Receiver="receiver@gmail.com", Subject = "Happy Birthday my friend", Content ="Hello your 30! Wish you see many many lucky with this old, happiness and healthy", ImageNameTrans="image1.jpg", TimeSend = DateTime.Now, NameCard="eGreeting-Birthday-001" }
            };
            dsTrans.ForEach(item => context.Transactions.Add(item));


            // Seed some record for Users
            var dsUser = new List<User>
            {
                new User { UserName = "admin" ,Password = "admin1234",RePassword = "admin1234", FullName = "Admin", Gender = true, Email ="admin@egreeting.com",Phone = "0762327226", Role = true},
                new User { UserName = "test" ,Password = "123123123",RePassword = "123123123", FullName = "test", Gender = true, Email ="test@gmail.com",Phone = "0762371254"}
            };
            dsUser.ForEach(item => context.Users.Add(item));
            context.SaveChanges();
        }
    }

   
}