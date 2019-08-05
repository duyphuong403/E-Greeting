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

        public virtual DbSet<EmailList> EmailLists { get; set; }

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
                new Card { NameCard="Birthday-01",Category="Birthday",ImageName="b-image1.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-02",Category="Birthday",ImageName="b-image2.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-03",Category="Birthday",ImageName="b-image3.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-04",Category="Birthday",ImageName="b-image4.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-05",Category="Birthday",ImageName="b-image5.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-06",Category="Birthday",ImageName="b-image6.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-07",Category="Birthday",ImageName="b-image7.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-08",Category="Birthday",ImageName="b-image8.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-09",Category="Birthday",ImageName="b-image9.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-10",Category="Birthday",ImageName="b-image10.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-11",Category="Birthday",ImageName="b-image11.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-12",Category="Birthday",ImageName="b-image12.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-13",Category="Birthday",ImageName="b-image13.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-14",Category="Birthday",ImageName="b-image14.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-15",Category="Birthday",ImageName="b-image15.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-16",Category="Birthday",ImageName="b-image16.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-17",Category="Birthday",ImageName="b-image17.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Birthday-18",Category="Birthday",ImageName="b-image18.jpg", DateCreated = DateTime.Now},
                
                new Card { NameCard="NewYear-01",Category="NewYear",ImageName="n-image1.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-02",Category="NewYear",ImageName="n-image2.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-03",Category="NewYear",ImageName="n-image3.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-04",Category="NewYear",ImageName="n-image4.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-05",Category="NewYear",ImageName="n-image5.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-06",Category="NewYear",ImageName="n-image6.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-07",Category="NewYear",ImageName="n-image7.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-08",Category="NewYear",ImageName="n-image8.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-09",Category="NewYear",ImageName="n-image9.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-10",Category="NewYear",ImageName="n-image10.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-11",Category="NewYear",ImageName="n-image11.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-12",Category="NewYear",ImageName="n-image12.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-13",Category="NewYear",ImageName="n-image13.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-14",Category="NewYear",ImageName="n-image14.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-15",Category="NewYear",ImageName="n-image15.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-16",Category="NewYear",ImageName="n-image16.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-17",Category="NewYear",ImageName="n-image17.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="NewYear-18",Category="NewYear",ImageName="n-image18.jpg", DateCreated = DateTime.Now},

                new Card { NameCard="Festival-01",Category="Festival",ImageName="f-image1.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-02",Category="Festival",ImageName="f-image2.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-03",Category="Festival",ImageName="f-image3.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-04",Category="Festival",ImageName="f-image4.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-05",Category="Festival",ImageName="f-image5.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-06",Category="Festival",ImageName="f-image6.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-07",Category="Festival",ImageName="f-image7.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-08",Category="Festival",ImageName="f-image8.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-09",Category="Festival",ImageName="f-image9.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-10",Category="Festival",ImageName="f-image10.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-11",Category="Festival",ImageName="f-image11.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-12",Category="Festival",ImageName="f-image12.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-13",Category="Festival",ImageName="f-image13.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-14",Category="Festival",ImageName="f-image14.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-15",Category="Festival",ImageName="f-image15.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-16",Category="Festival",ImageName="f-image16.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-17",Category="Festival",ImageName="f-image17.jpg", DateCreated = DateTime.Now},
                new Card { NameCard="Festival-18",Category="Festival",ImageName="f-image18.jpg", DateCreated = DateTime.Now},
            };
            ds.ForEach(item => context.Cards.Add(item));

            // Seed for Trans
            var dsTrans = new List<Transaction>
            {
                new Transaction { Username = "test", Receiver="receiver@gmail.com", Subject = "Happy Birthday my friend", Content ="Hello your 30! Wish you see many many lucky with this old, happiness and healthy", ImageNameTrans="image1.jpg", TimeSend = DateTime.Now, NameCard="Birthday-01" }
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