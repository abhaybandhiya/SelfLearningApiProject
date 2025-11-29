using AutoMapper;
using SelfLearningApiProject.Entities;
using SelfLearningApiProject.Models.DTO;
using SelfLearningApiProject.Repositories;

namespace SelfLearningApiProject.Services
{
    // Yeh class service layer ka kaam karti hai - business logic yahi hoti hai
    public class ProductService : IProductService
    {
        // Repository ka reference, jisse database ke sath communication hoga
        private readonly IProductRepository _productRepository;

        // Mapper ka reference, jisse entity ko DTO me convert karte hain (agar zarurat ho to)
        private readonly IMapper _mapper;

        // Constructor me dependency injection ke through repository milti hai
        public ProductService(IProductRepository productRepository,IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper; // Mapper ko initialize karte hain, jisse entity se DTO me conversion ho sake
        }

        // Method: Saare products fetch karta hai, unhe DTO me convert karta hai aur return karta hai
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            // Repository se entity list laate hain (Product entity list)
            var products = await _productRepository.GetAllAsync();

            // Entity list ko DTO me convert karte hain (Client ko sirf required fields dikhani chahiye)
            //return products.Select(p => new ProductDto
            //{
            //    Id = p.Id,       // Product entity ka Id field DTO me map
            //    Name = p.Name  , // Product entity ka Name field DTO me map
            //    Price = p.Price  // Product entity ka Price field DTO me map
            //});

            // Mapper ka use karke entity list ko DTO list me convert karte hain
            return _mapper.Map<IEnumerable<ProductDto>>(products); // yeh line Product entity list ko ProductDto list me convert karti hai // aur return karti hai // agar products null nahi hai to, to yeh IEnumerable<ProductDto> object return karega // agar products null hai to, to yeh null return karega
        }

        // Method: Ek product ko ID ke basis pe laata hai, aur usse DTO me convert karta hai
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {        
            try
            {
                // Repository se ek product laate hain (ya null agar nahi mila)
                var product = await _productRepository.GetByIdAsync(id);

                // Agar product null mila (matlab DB me nahi mila), to null return karo
                if (product == null)
                    return null;

                // Agar mila to usse DTO format me convert karke return karo
                //return new ProductDto
                //{
                //    Id = product.Id,
                //    Name = product.Name
                //};

                // Mapper ka use karke entity ko DTO me convert karte hain
                return _mapper.Map<ProductDto>(product); // yeh line Product entity ko ProductDto me convert karti hai // aur return karti hai // agar product null nahi hai to, to yeh ProductDto object return karega // agar product null hai to, to yeh null return karega

            }
            catch (Exception ex)
            {
                // Yahan pe log karna best hota hai (file ya console me)
                Console.WriteLine($"Error in GetProductByIdAsync: {ex.Message}");

                // Service me exception ko re-throw karte hain
                throw; // Ye controller tak error pass karega
            }
        }

        // Method: Naya product create karta hai aur uska DTO return karta hai
        public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            // DTO se ek naya Entity object banate hain (yeh DB me store hoga)
            var product = new Product
            {
                Name = productDto.Name, // Name ko set karte hain jo client se aaya hai
                 //  Id = productDto.Id, // Agar Id client se aayi hai to use set karte hain, warna auto-generated ID use karenge
                Price = productDto.Price // Price bhi set karte hain agar DTO me diya hai
            };

            // Repository ko bolte hain naya product add karne ke liye
            await _productRepository.AddAsync(product);

            // DB me changes save karna zaruri hai
            await _productRepository.SaveChangesAsync();

            // Entity se DTO banake return karte hain (confirmation ke liye)
            //return new ProductDto
            //{
            //    //Id = product.Id, // Id bhi DTO me set karte hain, yeh auto-generated ID hoti hai
            //    Name = product.Name, // Name bhi DTO me set karte hain
            //    Price = product.Price // Price bhi DTO me set karte hain
            //};

            // Mapper ka use karke entity ko DTO me convert karte hain
            return _mapper.Map<ProductDto>(product); // yeh line Product entity ko ProductDto me convert karti hai // aur return karti hai // agar product null nahi hai to, to yeh ProductDto object return karega // agar product null hai to, to yeh null return karega
        }

        // Method: Ek existing product ko update karta hai ID ke basis pe
        public async Task<bool> UpdateProductAsync(int id, ProductDto productDto)
        {
            // DB se existing product nikalte hain
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false; // agar nahi mila to false return karo

            // Entity ko update karte hain DTO ke values se
            //product.Name = productDto.Name;
            //product.Price = productDto.Price;

            // Mapper ka use karke productDto se product entity ko update karte hain // yeh line ProductDto object ko Product entity me map karti hai // aur product entity ko update karti hai // agar productDto null nahi hai to, to yeh Product entity ko update karega // agar productDto null hai to, to yeh Product entity ko update nahi karega
            _mapper.Map(productDto, product);

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

        // Method: Pagination ke liye products fetch karta hai specific page aur size ke hisaab se
        public async Task<IEnumerable<ProductDto>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetPaginatedProductsAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // Method: Keyword ke basis pe products search karta hai
        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string keyword)
        {
            // Repository se search results laate hain
            var products = await _productRepository.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        // Method: Products ko sort karta hai given field aur order ke hisaab se
        public async Task<IEnumerable<ProductDto>> GetSortedProductsAsync(string sortBy, string sortOrder)
        {
            var products = await _productRepository.SortAsync(sortBy, sortOrder);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

    }
}
