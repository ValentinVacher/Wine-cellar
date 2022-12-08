using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using Wine_cellar.Entities;

namespace Wine_cellar.Contexts
{
    public class WineContext : DbContext
    {
        public WineContext(DbContextOptions<WineContext> dbContextOptions) : base(dbContextOptions)
        {
            // Force pending migrations
            //Database.Migrate();
        }
        public DbSet<Cellar> Cellars { get; set; }
        public DbSet<Drawer> Drawers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Wine> Wines { get; set; }
        public DbSet<Appelation> Appelations { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var c1 = new Cellar { CellarId = 1, Name = "Cellar 1", NbDrawerMax = 5, UserId = 1 };
            var c2 = new Cellar { CellarId = 2, Name = "Cellar 2", NbDrawerMax = 10, UserId = 2 };
            var c3 = new Cellar { CellarId = 3, Name = "Cellar 3", NbDrawerMax = 20, UserId = 3 };

            var d1 = new Drawer { DrawerId = 1, Index = 1, CellarId = 1, NbBottleMax = 5 };
            var d2 = new Drawer { DrawerId = 2, Index = 2, CellarId = 1, NbBottleMax = 5 };
            var d3 = new Drawer { DrawerId = 3, Index = 3, CellarId = 1, NbBottleMax = 5 };
            var d4 = new Drawer { DrawerId = 4, Index = 1, CellarId = 2, NbBottleMax = 5 };
            var d5 = new Drawer { DrawerId = 5, Index = 2, CellarId = 2, NbBottleMax = 5 };
            var d6 = new Drawer { DrawerId = 6, Index = 3, CellarId = 2, NbBottleMax = 5 };

            var v1 = new Wine { WineId = 1, Name = "20-1", Year = 1960, DrawerId = 1, PictureName = "", Color = WineColor.Rouge, AppelationId = 1 };
            var v2 = new Wine { WineId = 2, Name = "20-2", Year = 1970, DrawerId = 1, PictureName = "img/vin1.png", Color = WineColor.Rouge, AppelationId = 2 };
            var v3 = new Wine { WineId = 3, Name = "20-3", Year = 1980, DrawerId = 2, PictureName = "", Color = WineColor.Rouge, AppelationId = 3 };
            var v4 = new Wine { WineId = 4, Name = "20-4", Year = 1960, DrawerId = 2, PictureName = "", Color = WineColor.Rouge, AppelationId = 4 };
            var v5 = new Wine { WineId = 5, Name = "20-5", Year = 1960, DrawerId = 3, PictureName = "", Color = WineColor.Rouge, AppelationId = 5 };
            var v6 = new Wine { WineId = 6, Name = "20-6", Year = 1960, DrawerId = 3, PictureName = "", Color = WineColor.Rouge, AppelationId = 6 };
            var v7 = new Wine { WineId = 7, Name = "20-7", Year = 1960, DrawerId = 4, PictureName = "", Color = WineColor.Blanc, AppelationId = 12 };
            var v8 = new Wine { WineId = 8, Name = "20-8", Year = 1960, DrawerId = 4, PictureName = "", Color = WineColor.Blanc, AppelationId = 13 };
            var v9 = new Wine { WineId = 9, Name = "20-9", Year = 1960, DrawerId = 5, PictureName = "", Color = WineColor.Blanc, AppelationId = 14 };
            var v10 = new Wine { WineId = 10, Name = "20-10", Year = 1960, DrawerId = 5, PictureName = "", Color = WineColor.Blanc, AppelationId = 12 };
            var v11 = new Wine { WineId = 11, Name = "20-11", Year = 1960, DrawerId = 5, PictureName = "", Color = WineColor.Rosé, AppelationId = 26 };
            var v12 = new Wine { WineId = 12, Name = "20-12", Year = 1960, DrawerId = 5, PictureName = "", Color = WineColor.Rosé, AppelationId = 27 };


            var u1 = new User { UserId = 1, FirstName = "G", LastName = "G", Email = "test@test.com", Password = "test" };
            var u2 = new User { UserId = 2, FirstName = "G2", LastName = "G2", Email = "test2@test.com", Password = "test2" };
            var u3 = new User { UserId = 3, FirstName = "G3", LastName = "G3", Email = "test3@test.com", Password = "test3" };


            var a1 = new Appelation { AppelationId = 1, AppelationName = "Bordeaux", KeepMin = 5, KeepMax = 10, Color = WineColor.Rouge };
            var a2 = new Appelation { AppelationId = 2, AppelationName = "Loire", KeepMin = 4, KeepMax = 6, Color = WineColor.Rouge };
            var a3 = new Appelation { AppelationId = 3, AppelationName = "Bordeaux, Grands crus", KeepMin = 10, KeepMax = 20, Color = WineColor.Rouge };
            var a4 = new Appelation { AppelationId = 4, AppelationName = "Sud-Ouest", KeepMin = 5, KeepMax = 10, Color = WineColor.Rouge };
            var a5 = new Appelation { AppelationId = 5, AppelationName = "Languedoc & Provence", KeepMin = 5, KeepMax = 8, Color = WineColor.Rouge };
            var a6 = new Appelation { AppelationId = 6, AppelationName = "Côtes du Rhône", KeepMin = 4, KeepMax = 6, Color = WineColor.Rouge };
            var a7 = new Appelation { AppelationId = 7, AppelationName = "Cotes du Rhônes, Grands Crus", KeepMin = 10, KeepMax = 20, Color = WineColor.Rouge };
            var a8 = new Appelation { AppelationId = 8, AppelationName = "Beaujolais", KeepMin = 4, KeepMax = 5, Color = WineColor.Rouge };
            var a9 = new Appelation { AppelationId = 9, AppelationName = "Beaujolais, Crus", KeepMin = 5, KeepMax = 8, Color = WineColor.Rouge };
            var a10 = new Appelation { AppelationId = 10, AppelationName = "Bourgogne, Saône-et-Loire", KeepMin = 5, KeepMax = 10, Color = WineColor.Rouge };
            var a11 = new Appelation { AppelationId = 11, AppelationName = "Bourgogne, Côte-d'Or", KeepMin = 10, KeepMax = 20, Color = WineColor.Rouge };
            var a12 = new Appelation { AppelationId = 12, AppelationName = "Loire, Sec", KeepMin = 3, KeepMax = 4, Color = WineColor.Blanc };
            var a13 = new Appelation { AppelationId = 13, AppelationName = "Loire, moelleux et liquoreux", KeepMin = 10, KeepMax = 20, Color = WineColor.Blanc };
            var a14 = new Appelation { AppelationId = 14, AppelationName = "Bordeaux, sec", KeepMin = 5, KeepMax = 8, Color = WineColor.Blanc };
            var a15 = new Appelation { AppelationId = 15, AppelationName = "Bordeaux, liquoreux", KeepMin = 15, KeepMax = 20, Color = WineColor.Blanc };
            var a16 = new Appelation { AppelationId = 16, AppelationName = "Sud-Ouest, sec", KeepMin = 4, KeepMax = 5, Color = WineColor.Blanc };
            var a17 = new Appelation { AppelationId = 17, AppelationName = "Sud-Ouest, liquoreux", KeepMin = 10, KeepMax = 15, Color = WineColor.Blanc };
            var a18 = new Appelation { AppelationId = 18, AppelationName = "Languedoc & Provence", KeepMin = 3, KeepMax = 3, Color = WineColor.Blanc };
            var a19 = new Appelation { AppelationId = 19, AppelationName = "Côtes du Rhône", KeepMin = 3, KeepMax = 4, Color = WineColor.Blanc };
            var a20 = new Appelation { AppelationId = 20, AppelationName = "Bourgogne, Saône-et-Loire", KeepMin = 4, KeepMax = 4, Color = WineColor.Blanc };
            var a21 = new Appelation { AppelationId = 21, AppelationName = "Bourgogne, Côte-d'Or", KeepMin = 7, KeepMax = 10, Color = WineColor.Blanc };
            var a22 = new Appelation { AppelationId = 22, AppelationName = "Jura", KeepMin = 8, KeepMax = 20, Color = WineColor.Blanc };
            var a23 = new Appelation { AppelationId = 23, AppelationName = "Jura", KeepMin = 8, KeepMax = 20, Color = WineColor.Blanc };
            var a24 = new Appelation { AppelationId = 24, AppelationName = "Alsace", KeepMin = 4, KeepMax = 5, Color = WineColor.Blanc };
            var a25 = new Appelation { AppelationId = 25, AppelationName = "Languedoc", KeepMin = 3, KeepMax = 4, Color = WineColor.Rosé };
            var a26 = new Appelation { AppelationId = 26, AppelationName = "Provence", KeepMin = 3, KeepMax = 3, Color = WineColor.Rosé };
            var a27 = new Appelation { AppelationId = 27, AppelationName = "Rhône", KeepMin = 2, KeepMax = 2, Color = WineColor.Rosé };


            modelBuilder.Entity<Cellar>().HasData(new List<Cellar> { c1, c2, c3 });
            modelBuilder.Entity<Drawer>().HasData(new List<Drawer> { d1, d2, d3, d4, d5, d6 });
            modelBuilder.Entity<Wine>().HasData(new List<Wine> { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10 });
            modelBuilder.Entity<User>().HasData(new List<User> { u1, u2, u3 });
            modelBuilder.Entity<Appelation>().HasData(new List<Appelation> { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, 
            a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27});
          

            base.OnModelCreating(modelBuilder);
        }
    }
}
