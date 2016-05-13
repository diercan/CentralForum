using Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Context
{
    public class ApplicationDbContext : DbContext
    {
        private static ApplicationDbContext instance;

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public DbSet<Rating> Rating { get; set; }
        public DbSet<Message> Message { get; set; }

        public static ApplicationDbContext CreateContext()
        {
            if (instance == null)
            {
                instance = new ApplicationDbContext();
            }
            return instance;
        }
    }
}
