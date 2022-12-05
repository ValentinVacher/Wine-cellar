using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Wine_celar.Entities;

namespace Wine_celar.Contexts
{
    public class WineContext : DbContext
    {
        public WineContext(DbContextOptions<WineContext> dbContextOptions) : base(dbContextOptions)
        {
            // Force pending migrations
            Database.Migrate();
        }
        public DbSet<Celar> Celars { get; set; }
        public DbSet<Drawer> Drawers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wine> Wines { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Config set in Program.cs
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Projet_API_Wiky;Trusted_Connection=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
