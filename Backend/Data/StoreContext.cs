using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization.Internal;
using System.Reflection;

namespace Backend.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) {}

        public DbSet<TransactionMD> TransactionMDs { get; set; }


        /* override builder for working with our custom config files  */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
