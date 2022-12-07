﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        public DbSet<ColorWine> ColorWines { get; set; }
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

            var v1 = new Wine { WineId = 1, Name = "20-1", Year = 1960, DrawerId = 1, PictureName = "" };
            var v2 = new Wine { WineId = 2, Name = "20-2", Year = 1970, DrawerId = 1, PictureName = "img/vin1.png" };
            var v3 = new Wine { WineId = 3, Name = "20-3", Year = 1980, DrawerId = 2, PictureName = "" };
            var v4 = new Wine { WineId = 4, Name = "20-4", Year = 1960, DrawerId = 2, PictureName = "" };
            var v5 = new Wine { WineId = 5, Name = "20-5", Year = 1960, DrawerId = 3, PictureName = "" };
            var v6 = new Wine { WineId = 6, Name = "20-6", Year = 1960, DrawerId = 3, PictureName = "" };
            var v7 = new Wine { WineId = 7, Name = "20-7", Year = 1960, DrawerId = 4, PictureName = "" };
            var v8 = new Wine { WineId = 8, Name = "20-8", Year = 1960, DrawerId = 4, PictureName = "" };
            var v9 = new Wine { WineId = 9, Name = "20-9", Year = 1960, DrawerId = 5, PictureName = "" };
            var v10 = new Wine { WineId = 10, Name = "20-10", Year = 1960, DrawerId = 5, PictureName = "" };
            var v11 = new Wine { WineId = 11, Name = "20-11", Year = 1960, DrawerId = 5, PictureName = "" };
            var v12 = new Wine { WineId = 12, Name = "20-12", Year = 1960, DrawerId = 5, PictureName = "" };


            var u1 = new User { UserId = 1, FirstName = "G", LastName = "G", Email = "test@test.com", Password = "test" };
            var u2 = new User { UserId = 2, FirstName = "G2", LastName = "G2", Email = "test2@test.com", Password = "test2" };
            var u3 = new User { UserId = 3, FirstName = "G3", LastName = "G3", Email = "test3@test.com", Password = "test3" };

            var cw1 = new ColorWine { ColorId = 1, Name = "Rouge" };
            var cw2 = new ColorWine { ColorId = 2, Name = "Blanc" };
            var cw3 = new ColorWine { ColorId = 3, Name = "Rosé" };
            var cw4 = new ColorWine { ColorId = 4, Name = "Vert" };

            var a1 = new Appelation { AppelationId = 1, AppelationName = "Bordeaux", KeepMin = 5, KeepMax = 10 };
            var a2 = new Appelation { AppelationId = 2, AppelationName = "Loire", KeepMin = 4, KeepMax = 6 };
            var a3 = new Appelation { AppelationId = 3, AppelationName = "Bordeaux, Grands crus", KeepMin = 10, KeepMax = 20 };
            var a4 = new Appelation { AppelationId = 4, AppelationName = "Sud-Ouest", KeepMin = 5, KeepMax = 10 };
            var a5 = new Appelation { AppelationId = 5, AppelationName = "Languedoc & Provence", KeepMin = 5, KeepMax = 8 };
            var a6 = new Appelation { AppelationId = 6, AppelationName = "Côtes du Rhône", KeepMin = 4, KeepMax = 6 };


            modelBuilder.Entity<Cellar>().HasData(new List<Cellar> { c1, c2, c3 });
            modelBuilder.Entity<Drawer>().HasData(new List<Drawer> { d1, d2, d3, d4, d5, d6 });
            modelBuilder.Entity<Wine>().HasData(new List<Wine> { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10 });
            modelBuilder.Entity<User>().HasData(new List<User> { u1, u2, u3 });
            modelBuilder.Entity<Appelation>().HasData(new List<Appelation> { a1, a2, a3, a4, a5, a6});
            modelBuilder.Entity<ColorWine>().HasData(new List<ColorWine> {cw1, cw2, cw3, cw4 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
