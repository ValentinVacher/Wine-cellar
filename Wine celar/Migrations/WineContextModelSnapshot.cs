﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wine_cellar.Contexts;

#nullable disable

namespace Winecelar.Migrations
{
    [DbContext(typeof(WineContext))]
    partial class WineContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Wine_cellar.Entities.Cellar", b =>
                {
                    b.Property<int>("CellarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CellarId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NbDrawerMax")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CellarId");

                    b.HasIndex("UserId");

                    b.ToTable("Cellars");

                    b.HasData(
                        new
                        {
                            CellarId = 1,
                            Name = "Cellar 1",
                            NbDrawerMax = 5,
                            UserId = 1
                        },
                        new
                        {
                            CellarId = 2,
                            Name = "Cellar 2",
                            NbDrawerMax = 10,
                            UserId = 2
                        },
                        new
                        {
                            CellarId = 3,
                            Name = "Cellar 3",
                            NbDrawerMax = 20,
                            UserId = 3
                        });
                });

            modelBuilder.Entity("Wine_cellar.Entities.Drawer", b =>
                {
                    b.Property<int>("DrawerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DrawerId"));

                    b.Property<int>("CellarId")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("NbBottleMax")
                        .HasColumnType("int");

                    b.HasKey("DrawerId");

                    b.HasIndex("CellarId");

                    b.ToTable("Drawers");

                    b.HasData(
                        new
                        {
                            DrawerId = 1,
                            CellarId = 1,
                            Index = 1,
                            NbBottleMax = 5
                        },
                        new
                        {
                            DrawerId = 2,
                            CellarId = 1,
                            Index = 2,
                            NbBottleMax = 5
                        },
                        new
                        {
                            DrawerId = 3,
                            CellarId = 1,
                            Index = 3,
                            NbBottleMax = 5
                        },
                        new
                        {
                            DrawerId = 4,
                            CellarId = 2,
                            Index = 1,
                            NbBottleMax = 5
                        },
                        new
                        {
                            DrawerId = 5,
                            CellarId = 2,
                            Index = 2,
                            NbBottleMax = 5
                        },
                        new
                        {
                            DrawerId = 6,
                            CellarId = 2,
                            Index = 3,
                            NbBottleMax = 5
                        });
                });

            modelBuilder.Entity("Wine_cellar.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "test@test.com",
                            FirstName = "G",
                            LastName = "G",
                            Password = "test"
                        },
                        new
                        {
                            UserId = 2,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "test2@test.com",
                            FirstName = "G2",
                            LastName = "G2",
                            Password = "test2"
                        },
                        new
                        {
                            UserId = 3,
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "test3@test.com",
                            FirstName = "G3",
                            LastName = "G3",
                            Password = "test3"
                        });
                });

            modelBuilder.Entity("Wine_cellar.Entities.Wine", b =>
                {
                    b.Property<int>("WineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WineId"));

                    b.Property<string>("Appelation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DrawerId")
                        .HasColumnType("int");

                    b.Property<int>("KeepMax")
                        .HasColumnType("int");

                    b.Property<int>("KeepMin")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Today")
                        .HasColumnType("datetime2");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("WineId");

                    b.HasIndex("DrawerId");

                    b.ToTable("Wines");

                    b.HasData(
                        new
                        {
                            WineId = 1,
                            Appelation = "Appelation1",
                            Color = "Rosé",
                            DrawerId = 1,
                            KeepMax = 2002,
                            KeepMin = 2000,
                            Name = "20-1",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5509),
                            Year = 1960
                        },
                        new
                        {
                            WineId = 2,
                            Appelation = "Appelation2",
                            Color = "Bleu",
                            DrawerId = 1,
                            KeepMax = 2002,
                            KeepMin = 2001,
                            Name = "20-2",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5551),
                            Year = 1970
                        },
                        new
                        {
                            WineId = 3,
                            Appelation = "Appelation3",
                            Color = "Verre",
                            DrawerId = 2,
                            KeepMax = 2002,
                            KeepMin = 2001,
                            Name = "20-3",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5554),
                            Year = 1980
                        },
                        new
                        {
                            WineId = 4,
                            Appelation = "Appelation4",
                            Color = "Rouge",
                            DrawerId = 2,
                            KeepMax = 2002,
                            KeepMin = 2000,
                            Name = "20-4",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5556),
                            Year = 1960
                        },
                        new
                        {
                            WineId = 5,
                            Appelation = "Appelation5",
                            Color = "Jaune",
                            DrawerId = 3,
                            KeepMax = 2002,
                            KeepMin = 2000,
                            Name = "20-5",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5559),
                            Year = 1960
                        },
                        new
                        {
                            WineId = 6,
                            Appelation = "Appelation6",
                            Color = "Blanc",
                            DrawerId = 3,
                            KeepMax = 2002,
                            KeepMin = 2000,
                            Name = "20-6",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5561),
                            Year = 1960
                        },
                        new
                        {
                            WineId = 7,
                            Appelation = "Appelation7",
                            Color = "Rouge",
                            DrawerId = 4,
                            KeepMax = 2002,
                            KeepMin = 2000,
                            Name = "20-7",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5563),
                            Year = 1960
                        },
                        new
                        {
                            WineId = 8,
                            Appelation = "Appelation8",
                            Color = "Violet",
                            DrawerId = 4,
                            KeepMax = 2002,
                            KeepMin = 2000,
                            Name = "20-8",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5566),
                            Year = 1960
                        },
                        new
                        {
                            WineId = 9,
                            Appelation = "Appelation9",
                            Color = "Orange",
                            DrawerId = 5,
                            KeepMax = 2002,
                            KeepMin = 2000,
                            Name = "20-9",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5568),
                            Year = 1960
                        },
                        new
                        {
                            WineId = 10,
                            Appelation = "Appelation10",
                            Color = "Violet",
                            DrawerId = 5,
                            KeepMax = 2002,
                            KeepMin = 2000,
                            Name = "20-10",
                            Today = new DateTime(2022, 12, 5, 13, 24, 4, 16, DateTimeKind.Local).AddTicks(5570),
                            Year = 1960
                        });
                });

            modelBuilder.Entity("Wine_cellar.Entities.Cellar", b =>
                {
                    b.HasOne("Wine_cellar.Entities.User", "User")
                        .WithMany("Cellars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wine_cellar.Entities.Drawer", b =>
                {
                    b.HasOne("Wine_cellar.Entities.Cellar", "Cellar")
                        .WithMany("Drawers")
                        .HasForeignKey("CellarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cellar");
                });

            modelBuilder.Entity("Wine_cellar.Entities.Wine", b =>
                {
                    b.HasOne("Wine_cellar.Entities.Drawer", "Drawer")
                        .WithMany("Wines")
                        .HasForeignKey("DrawerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Drawer");
                });

            modelBuilder.Entity("Wine_cellar.Entities.Cellar", b =>
                {
                    b.Navigation("Drawers");
                });

            modelBuilder.Entity("Wine_cellar.Entities.Drawer", b =>
                {
                    b.Navigation("Wines");
                });

            modelBuilder.Entity("Wine_cellar.Entities.User", b =>
                {
                    b.Navigation("Cellars");
                });
#pragma warning restore 612, 618
        }
    }
}
