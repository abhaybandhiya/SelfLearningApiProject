
using Microsoft.EntityFrameworkCore;
using SelfLearningApiProject.Data;
using SelfLearningApiProject.Entities;

namespace SelfLearningApiProject.Repositories // Namespace define karta hai jahan humari repository classes hain
{
    // Yeh interface Product repository ke liye hai, jisse CRUD operations define hote hain
    public class ProductRepository : IProductRepository // Yeh class Product entity ke liye CRUD operations implement karti hai
    {
        // Yeh class AppDbContext ka use karti hai, jo ki Entity Framework Core ka DbContext hai // aur database operations ke liye use hota hai
        private readonly AppDbContext _context;

        // Constructor me AppDbContext ko inject karte hain, jisse database ke sath kaam kar sakein
        public ProductRepository(AppDbContext context)
        {
            // Yeh line AppDbContext ko initialize karti hai, jisse database operations perform kiye ja sakein
            _context = context;
        }

        // Yeh method saare products ko database se fetch karta hai aur unhe return karta hai
        public async Task<IEnumerable<Product>> GetAllAsync()
        { // Yeh method saare products ko database se fetch karta hai aur unhe return karta hai // yeh asynchronous hai, isliye await keyword use hota hai// aur Task<IEnumerable<Product>> return karta hai// jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho // yeh method database se saare products ko fetch karta hai aur unhe list me convert karke return karta hai
            return await _context.Products.ToListAsync();
        }

        // Yeh method specific product ko ID ke basis pe fetch karta hai aur return karta hai
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id); // Yeh method specific product ko ID ke basis pe fetch karta hai aur return karta hai // agar product nahi mila to yeh null return karega // yeh asynchronous hai, isliye await keyword use hota hai // aur Task<Product?> return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
        }

        // Yeh method naya product add karta hai database me 
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product); // Yeh method naya product add karta hai database me // yeh asynchronous hai, isliye await keyword use hota hai // aur Task return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
        }

        // Yeh method existing product ko update karta hai database me
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product); // Yeh method existing product ko update karta hai database me // yeh asynchronous hai, isliye await keyword use hota hai // aur Task return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
            await Task.CompletedTask; // just to make method async
        }

        // Yeh method product ko database se delete karta hai
        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product); // Yeh method product ko database se delete karta hai // yeh asynchronous hai, isliye await keyword use hota hai // aur Task return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
            await Task.CompletedTask;
        }

        // Yeh method changes ko database me save karta hai
        public async Task<bool> SaveChangesAsync() // Yeh method changes ko database me save karta hai // yeh asynchronous hai, isliye await keyword use hota hai // aur Task<bool> return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho // agar changes save ho gaye to true return karega, warna false
        {
            return await _context.SaveChangesAsync() > 0; // Yeh line changes ko database me save karti hai aur agar koi changes save hue to true return karti hai, warna false return karti hai // yeh asynchronous hai, isliye await keyword use hota hai // aur Task<bool> return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
        }
    }
}
