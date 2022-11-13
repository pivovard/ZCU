using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bank.Models;

namespace Bank.Models
{
    /// <summary>
    /// Database context of application
    /// </summary>
    public partial class BankContext : DbContext
    {
        /// <summary>
        /// Table of payments
        /// </summary>
        public DbSet<Bank.Models.Payment> Payment { get; set; }

        /// <summary>
        /// Table of templates
        /// </summary>
        public DbSet<Bank.Models.Template> Template { get; set; }

        /// <summary>
        /// Table of standing payments
        /// </summary>
        public DbSet<Bank.Models.Standing> Standing { get; set; }

        /// <summary>
        /// Table of users
        /// </summary>
        public DbSet<Bank.Models.User> User { get; set; }
        

        /// <param name="options"></param>
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => {
                entity.HasIndex(e => e.Login).IsUnique();
                //entity.HasIndex(e => e.AccountNumber).IsUnique();
                //entity.HasIndex(e => e.CardNumber).IsUnique();
            });
        }
    }
}
