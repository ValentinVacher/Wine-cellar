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
            //Database.Migrate();
        }
        public DbSet<Celar> Celars { get; set; }
        public DbSet<Drawer> Drawers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wine> Wines { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var c1 = new Celar { CelarId = 1, Name = "Celar 1", NbDrawerMax = 5, UserId = 1 };
            var c2 = new Celar { CelarId = 2, Name = "Celar 2", NbDrawerMax = 10, UserId = 2 };
            var c3 = new Celar { CelarId = 3, Name = "Celar 3", NbDrawerMax = 20, UserId = 3 };

            var d1 = new Drawer { DrawerId = 1, Index = 1, CelarId = 1 };
            var d2 = new Drawer { DrawerId = 2, Index = 2, CelarId = 1 };
            var d3 = new Drawer { DrawerId = 3, Index = 3, CelarId = 1 };
            var d4 = new Drawer { DrawerId = 4, Index = 1, CelarId = 2 };
            var d5 = new Drawer { DrawerId = 5, Index = 2, CelarId = 2 };
            var d6 = new Drawer { DrawerId = 6, Index = 3, CelarId = 2 };

            var v1 = new Wine { WineId = 1, Color = "Rosé", Appelation = "Appelation1", Name = "20-1", Year = 1960, Today = DateTime.Now, KeepMin = 2000, KeepMax = 2002, DrawerId = 1};
            var v2 = new Wine { WineId = 2, Color = "Bleu", Appelation = "Appelation2", Name = "20-2", Year = 1970, Today = DateTime.Now, KeepMin = 2001, KeepMax = 2002, DrawerId = 1 };
            var v3 = new Wine { WineId = 3, Color = "Verre", Appelation = "Appelation3", Name = "20-3", Year = 1980, Today = DateTime.Now, KeepMin = 2001, KeepMax = 2002, DrawerId = 2 };
            var v4 = new Wine { WineId = 4, Color = "Rouge", Appelation = "Appelation4", Name = "20-4", Year = 1960, Today = DateTime.Now, KeepMin = 2000, KeepMax = 2002, DrawerId = 2 };
            var v5 = new Wine { WineId = 5, Color = "Jaune", Appelation = "Appelation5", Name = "20-5", Year = 1960, Today = DateTime.Now, KeepMin = 2000, KeepMax = 2002, DrawerId = 3 };
            var v6 = new Wine { WineId = 6, Color = "Blanc", Appelation = "Appelation6", Name = "20-6", Year = 1960, Today = DateTime.Now, KeepMin = 2000, KeepMax = 2002, DrawerId = 3 };
            var v7 = new Wine { WineId = 7, Color = "Rouge", Appelation = "Appelation7", Name = "20-7", Year = 1960, Today = DateTime.Now, KeepMin = 2000, KeepMax = 2002, DrawerId = 4 };
            var v8 = new Wine { WineId = 8, Color = "Violet", Appelation = "Appelation8", Name = "20-8", Year = 1960, Today = DateTime.Now, KeepMin = 2000, KeepMax = 2002, DrawerId = 4 };
            var v9 = new Wine { WineId = 9, Color = "Orange", Appelation = "Appelation9", Name = "20-9", Year = 1960, Today = DateTime.Now, KeepMin = 2000, KeepMax = 2002, DrawerId = 5 };
            var v10 = new Wine { WineId = 10, Color = "Violet", Appelation = "Appelation10", Name = "20-10", Year = 1960, Today = DateTime.Now, KeepMin = 2000, KeepMax = 2002, DrawerId = 5 };

            var u1 = new User { UserId = 1, FirstName = "G", LastName = "G", Email = "test@test.com", Password = "test" };
            var u2 = new User { UserId = 2, FirstName = "G2", LastName = "G2", Email = "test2@test.com", Password = "test2" };
            var u3 = new User { UserId = 3, FirstName = "G3", LastName = "G3", Email = "test3@test.com", Password = "test3" };

            modelBuilder.Entity<Celar>().HasData(new List<Celar> { c1, c2, c3 });
            modelBuilder.Entity<Drawer>().HasData(new List<Drawer> { d1, d2, d3, d4, d5, d6 });
            modelBuilder.Entity<Wine>().HasData(new List<Wine> { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10 });
            modelBuilder.Entity<User>().HasData(new List<User> { u1, u2, u3 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
