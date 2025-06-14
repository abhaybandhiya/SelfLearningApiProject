using SelfLearningApiProject.Entities;

namespace SelfLearningApiProject.Repositories
{
    // Yeh interface Product repository ke liye hai, jisse CRUD operations define hote hain
    public interface IProductRepository // Yeh interface CRUD operations define karta hai jo Product entity ke liye use honge
    {
        //Yeh methods repository ke operations ko define karte hain. CRUD operations ke liye methods define karte hain
        Task<IEnumerable<Product>> GetAllAsync(); //Sabhi products laata hai
        Task<Product?> GetByIdAsync(int id); //Id ke hisaab se product laata hai
        Task AddAsync(Product product); //Naya product add karta hai
        Task UpdateAsync(Product product); //Product ko update karta hai
        Task DeleteAsync(Product product); //Product ko delete karta hai
        Task<bool> SaveChangesAsync(); //Changes database me save karta hai (best practice)
    }
}
