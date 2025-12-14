using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace מטלת_בית.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            //Database.SetInitializer(new MyDbInitializer());
        }

        public DbSet<Person> People { get; set; }
    }

    // Seeder
    public class MyDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            // add some initial data
            context.People.Add(new Person { FullName = "Alice", Phone = "0501234567", Email = "alice@example.com" });
            context.People.Add(new Person { FullName = "Bob", Phone = "0502345678", Email = "bob@example.com" });
            context.People.Add(new Person { FullName = "Charlie", Phone = "0503456789", Email = "charlie@example.com" });
            context.People.Add(new Person { FullName = "Diana", Phone = "0504567890", Email = "diana@example.com" });
            context.People.Add(new Person { FullName = "Eve", Phone = "0505678901", Email = "eve@example.com" });

            context.SaveChanges(); // save changes to the database
        }
    }
}