using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<TransactionMD> TransactionMDs { get; set; }
    }
}
