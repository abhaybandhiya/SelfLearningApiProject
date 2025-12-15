using Microsoft.EntityFrameworkCore;
using SelfLearningApiProject.Entities;
using SelfLearningApiProject.Services;

namespace SelfLearningApiProject.Data
{
    public class AppDbContext: DbContext
    {
        // Yeh constructor DbContextOptions ko pass karta hai, jo ki database connection aur configuration ke liye use hota hai
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Yeh property 'Products' table ko represent karti hai, jisme Product entities store hongi
        public DbSet<Product> Products { get; set; }  // This creates a 'Products' table
        public DbSet<User> Users { get; set; }  // ye Users table ke liye hai

        private readonly ICurrentUserService _currentUser;

        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUser)
    : base(options)
        {
            _currentUser = currentUser;
        }

        // Yeh method OnModelCreating ko override karta hai, jahan hum model configuration karte hain
        protected override void OnModelCreating(ModelBuilder modelBuilder) // Yeh method model ko create karne ke liye use hota hai // yahan hum seed data bhi daal sakte hain
        {
            base.OnModelCreating(modelBuilder); // Base class ki OnModelCreating ko call karte hain. yeh zaroori hai taaki base class ki configurations bhi apply ho sakein

            // Fix for decimal precision warning
            modelBuilder.Entity<Product>()
                        .Property(p => p.Price)
                        .HasPrecision(18, 2); // ⬅️ Yeh line Price column ke liye decimal(18,2) apply karegi
            
            // Seed default product data
            modelBuilder.Entity<Product>().HasData( // Yeh seed data hai jo database me initial products daal dega jab migration apply hoga
                new Product { Id = 1, Name = "Pen" , Price = 10.00M },
                new Product { Id = 2, Name = "Notebook" , Price = 15.00M },
                new Product { Id = 3, Name = "Laptop", Price = 18.00M }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "$2a$12$l6fM6kpo4EMtIrkaRyGW3.TowOQtXcqCrtl0Hk5Ih95XEgshdTFQW",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Username = "user",
                    Password = "$2a$12$y9ivYvovbImDl8xzPpZVWeK9wGaqYX/qo0P2gUxW8y.TQaLEi3iRW",
                    Role = "User"
                }
            );
            modelBuilder.Entity<Product>() // global query filter lagata hai taaki soft deleted products ko queries me na laaye matlab jinke IsDeleted = true hai unhe exclude kar de 
            .HasQueryFilter(p => !p.IsDeleted);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Product &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                    entry.Property("CreatedBy").CurrentValue = _currentUser.Username ?? "System";
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                    entry.Property("UpdatedBy").CurrentValue = _currentUser.Username ?? "System";
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
