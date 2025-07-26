using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using SemesterProj.Models;

namespace SemesterProj.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Auth> Auth { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Bid> Bids { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed admin user data
            modelBuilder.Entity<Auth>().HasData(
                new Auth
                {
                    Id = 1, // Provide a primary key
                    Username = "admin",
                    Email = "admin@gmail.com",
                    Password = "admin@123" // Use plaintext only for testing purposes
                }
            );
            

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.MinBid)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CurrentBid)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.BidIncrement)
                    .HasColumnType("decimal(18,2)");
            });
            // Configure Item-Category relationship
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany() // No navigation property in Categories
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Item-Seller relationship
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Seller)
                .WithMany() // No navigation property in Auth
                .HasForeignKey(i => i.SellerId)
                .OnDelete(DeleteBehavior.Restrict);


       //     modelBuilder.Entity<Bid>()
       //.HasOne<Auth>(b => b.User)
       //.WithMany()
       //.HasForeignKey(b => b.UserId)
       //.OnDelete(DeleteBehavior.Restrict);
        }

    }


    }

