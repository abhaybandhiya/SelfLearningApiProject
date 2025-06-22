using Microsoft.EntityFrameworkCore;
using SelfLearningApiProject.Entities;
using System.Collections.Generic;

namespace SelfLearningApiProject.Data
{
    public class AppDbContext: DbContext
    {
        // Yeh constructor DbContextOptions ko pass karta hai, jo ki database connection aur configuration ke liye use hota hai
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Yeh property 'Products' table ko represent karti hai, jisme Product entities store hongi
        public DbSet<Product> Products { get; set; }  // This creates a 'Products' table

        // Yeh method OnModelCreating ko override karta hai, jahan hum model configuration karte hain
        protected override void OnModelCreating(ModelBuilder modelBuilder) // Yeh method model ko create karne ke liye use hota hai // yahan hum seed data bhi daal sakte hain
        {
            base.OnModelCreating(modelBuilder); // Base class ki OnModelCreating ko call karte hain. yeh zaroori hai taaki base class ki configurations bhi apply ho sakein

            // 🔧 Fix for decimal precision warning
            modelBuilder.Entity<Product>()
                        .Property(p => p.Price)
                        .HasPrecision(18, 2); // ⬅️ Yeh line Price column ke liye decimal(18,2) apply karegi

            // Seed default product data
            modelBuilder.Entity<Product>().HasData( // Yeh seed data hai jo database me initial products daal dega jab migration apply hoga
                new Product { Id = 1, Name = "Pen" , Price = 10.00M },
                new Product { Id = 2, Name = "Notebook" , Price = 15.00M },
                new Product { Id = 3, Name = "Laptop", Price = 18.00M }
            );
        }
    }
}
