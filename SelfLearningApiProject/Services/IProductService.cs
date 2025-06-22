using SelfLearningApiProject.Entities;
using SelfLearningApiProject.Models.DTO;

namespace SelfLearningApiProject.Services
{
    // Yeh interface service layer ka kaam define karta hai - business logic yahi hoti hai 

    // interface ka use kew karte he ? // Interface ka use karne se hume flexibility milti hai ki hum alag-alag implementations bana sakte hain bina code ko badle.
    public interface IProductService
    {   
        // Saare products ka list return karega in DTO format
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(); // Saare products ko DTO format me laata hai

        // Ek specific product return karega by ID (null agar nahi mila)
        Task<ProductDto?> GetProductByIdAsync(int id); // ID ke hisaab se product laata hai, agar nahi mila to null return karega

        // Yeh method naya product create karega aur uska DTO return karega.
        Task<ProductDto> CreateProductAsync(ProductDto productDto);

        // Product ko update karega aur updated DTO return karega
        Task<bool> UpdateProductAsync(int id, ProductDto productDto); // Product ko update karega by ID, agar successful hua to true return karega. agar nahi mila to false return karega

        // Product ko delete karega by ID
        Task<bool> DeleteProductAsync(int id); // Product ko delete karega by ID, agar successful hua to true return karega. agar nahi mila to false return karega

    }
}
