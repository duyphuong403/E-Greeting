namespace Application.eGreeting.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

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

    }

    public class DBInit : DropCreateDatabaseIfModelChanges<eGreetingDB>
    {
        protected override void Seed(eGreetingDB context)
        {
            // Seed some record for Cards
            var ds = new List<Card>
            {
                new Card { NameCard="eGreeting-Birthday-001",Category="Birthday",ImageName="image1.jpg"},
            };
            ds.ForEach(item => context.Cards.Add(item));
            //context.SaveChanges();

            // Seed some record for Users
            var dsUser = new List<User>
            {
                new User { UserName = "admin" ,Password = "admin1234",RePassword = "admin1234", FullName = "Admin", Gender = true, Email ="admin@egreeting.com",Phone = 0762327226, Role = true},
                new User { UserName = "test" ,Password = "12345678",RePassword = "12345678", FullName = "test", Gender = true, Email ="test@gmail.com",Phone = 0762371254}
            };
            dsUser.ForEach(item => context.Users.Add(item));
            context.SaveChanges();
        }
    }

   
}