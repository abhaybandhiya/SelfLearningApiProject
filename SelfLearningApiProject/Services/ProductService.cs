using SelfLearningApiProject.Entities;
using SelfLearningApiProject.Models;
using SelfLearningApiProject.Repositories;

namespace SelfLearningApiProject.Services
{
    // Yeh class service layer ka kaam karti hai - business logic yahi hoti hai
    public class ProductService : IProductService
    {
        // Repository ka reference, jisse database ke sath communication hoga
        private readonly IProductRepository _productRepository;

        // Constructor me dependency injection ke through repository milti hai
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Method: Saare products fetch karta hai, unhe DTO me convert karta hai aur return karta hai
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            // Repository se entity list laate hain (Product entity list)
            var products = await _productRepository.GetAllAsync();

            // Entity list ko DTO me convert karte hain (Client ko sirf required fields dikhani chahiye)
            return products.Select(p => new ProductDto
            {
                Id = p.Id,       // Product entity ka Id field DTO me map
                Name = p.Name    // Product entity ka Name field DTO me map
            });
        }

        // Method: Ek product ko ID ke basis pe laata hai, aur usse DTO me convert karta hai
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            // Repository se ek product laate hain (ya null agar nahi mila)
            var product = await _productRepository.GetByIdAsync(id);

            // Agar product null mila (matlab DB me nahi mila), to null return karo
            if (product == null)
                return null;

            // Agar mila to usse DTO format me convert karke return karo
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name
            };
        }

        // Method: Naya product create karta hai aur uska DTO return karta hai
        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            // DTO se ek naya Entity object banate hain (yeh DB me store hoga)
            var product = new Product
            {
                Name = productDto.Name
            };

            // Repository ko bolte hain naya product add karne ke liye
            await _productRepository.AddAsync(product);

            // DB me changes save karna zaruri hai
            await _productRepository.SaveChangesAsync();

            // Entity se DTO banake return karte hain (confirmation ke liye)
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name
            };
        }

        // Method: Ek existing product ko update karta hai ID ke basis pe
        public async Task<bool> UpdateProductAsync(int id, ProductDto productDto)
        {
            // DB se existing product nikalte hain
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false; // agar nahi mila to false return karo

            // Entity ko update karte hain DTO ke values se
            product.Name = productDto.Name;

            // Repository ko bolte hain update kar do
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();

            return true;
        }

        // Method: Ek product ko delete karta hai ID ke basis pe
        public async Task<bool> DeleteProductAsync(int id) // agar successful hua to true return karega. agar nahi mila to false return karega
        {
            // DB se product nikalte hain ID ke basis pe
            var product = await _productRepository.GetByIdAsync(id);

            // Agar product nahi mila, to false return karo
            if (product == null)
                return false;
            // Agar mila, to usse delete karte hain
            await _productRepository.DeleteAsync(product);
            // DB me changes save karna zaruri hai
            await _productRepository.SaveChangesAsync();

            return true;
        }

    }
}
