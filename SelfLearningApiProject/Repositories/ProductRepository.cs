
using Microsoft.EntityFrameworkCore;
using SelfLearningApiProject.Data;
using SelfLearningApiProject.Entities;
using SelfLearningApiProject.Models.DTO;

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
        { // Yeh method saare products ko database se fetch karta hai aur unhe return karta hai yeh asynchronous hai, isliye await keyword use hota hai// aur Task<IEnumerable<Product>> return karta hai// jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho // yeh method database se saare products ko fetch karta hai aur unhe list me convert karke return karta hai
            return await _context.Products.ToListAsync();
        }

        // Yeh method specific product ko ID ke basis pe fetch karta hai aur return karta hai
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id); // Yeh method specific product ko ID ke basis pe fetch karta hai aur return karta hai // agar product nahi mila to yeh null return karega // yeh asynchronous hai, isliye await keyword use hota hai // aur Task<Product?> return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na hoz
        }

        // Yeh method naya product add karta hai database me 
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product); // Yeh method naya product add karta hai database me // yeh asynchronous hai, isliye await keyword use hota hai // aur Task return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
            await _context.SaveChangesAsync();  
        }

        // Yeh method existing product ko update karta hai database me
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product); // Yeh method existing product ko update karta hai database me // yeh asynchronous hai, isliye await keyword use hota hai // aur Task return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
            await Task.CompletedTask; // just to make method async
            await _context.SaveChangesAsync();
        }

        // Yeh method product ko database se delete karta hai
        //public async Task DeleteAsync(Product product)
        //{
        //    _context.Products.Remove(product); // Yeh method product ko database se delete karta hai // yeh asynchronous hai, isliye await keyword use hota hai // aur Task return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
        //    await Task.CompletedTask;
        //}
        // Yeh method product ko soft delete karta hai database me yani product ko permanently delete nahi karta, balki uske IsDeleted flag ko true kar deta hai
        public async Task SoftDeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return;

            product.IsDeleted = true;       // Step 1: Soft delete flag
            _context.Products.Update(product);
            await _context.SaveChangesAsync();  // Step 2: Save changes
        }

        // Yeh method changes ko database me save karta hai
        public async Task<bool> SaveChangesAsync() // Yeh method changes ko database me save karta hai // yeh asynchronous hai, isliye await keyword use hota hai // aur Task<bool> return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho // agar changes save ho gaye to true return karega, warna false
        {
            return await _context.SaveChangesAsync() > 0; // Yeh line changes ko database me save karti hai aur agar koi changes save hue to true return karti hai, warna false return karti hai // yeh asynchronous hai, isliye await keyword use hota hai // aur Task<bool> return karta hai // jisse ki yeh non-blocking operation ho sake // aur UI thread block na ho
        }

        // Yeh method paginated products ko fetch karta hai database se
        public async Task<IEnumerable<Product>> GetPaginatedProductsAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Yeh method products ko keyword ke basis pe search karta hai
        //public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        //{
        //    return await _context.Products
        //        .Where(p => p.Name.Contains(keyword))   // Name me match karega
        //        .ToListAsync();
        //}

        // Yeh method products ko keyword ke basis pe search karta hai (LIKE operator ka use karke)
        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        {
            return await _context.Products.Where(p => EF.Functions.Like(p.Name, $"%{keyword}%")).ToListAsync();
        }

        // Yeh method products ko sort karta hai specified field aur order ke basis pe
        public async Task<IEnumerable<Product>> SortAsync(string sortBy, string sortOrder)
        {
            IQueryable<Product> query = _context.Products;

            // sortBy: name / price
            // sortOrder: asc / desc
            switch (sortBy.ToLower())
            {
                case "name":
                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name);
                    break;

                case "price":
                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price);
                    break;

                default:
                    // default sorting by Id
                    query = query.OrderBy(p => p.Id);
                    break;
            }

            return await query.ToListAsync();
        }

        // Yeh method products ko filter karta hai multiple criteria ke basis pe // jaise price range, name starts with, category, etc.
        public async Task<IEnumerable<Product>> FilterAsync(FilteringRequestDTO request)
        {
            IQueryable<Product> query = _context.Products;

            if (request.PriceFrom.HasValue)
                query = query.Where(p => p.Price >= request.PriceFrom);

            if (request.PriceTo.HasValue)
                query = query.Where(p => p.Price <= request.PriceTo);

            if (!string.IsNullOrWhiteSpace(request.NameStartsWith))
                query = query.Where(p => EF.Functions.Like(p.Name, $"{request.NameStartsWith}%"));

            // future ready (if category column added later)
            if (!string.IsNullOrWhiteSpace(request.Category))
                query = query.Where(p => p.Category == request.Category);

            return await query.ToListAsync();
        }

        // Yeh method advanced querying karta hai products pe // jisme searching, filtering, sorting, aur pagination sab included hai
        public async Task<(IEnumerable<Product>, int)> GetAdvancedAsync(AdvancedProductQueryDTO query)
        {
            IQueryable<Product> q = _context.Products;

            // Searching
            if (!string.IsNullOrWhiteSpace(query.Search))
                q = q.Where(p => EF.Functions.Like(p.Name, $"%{query.Search}%"));

            // Filtering
            if (query.PriceFrom.HasValue)
                q = q.Where(p => p.Price >= query.PriceFrom);

            if (query.PriceTo.HasValue)
                q = q.Where(p => p.Price <= query.PriceTo);

            // Sorting
            switch (query.SortBy.ToLower())
            {
                case "price":
                    q = query.SortOrder == "desc" ? q.OrderByDescending(p => p.Price) : q.OrderBy(p => p.Price);
                    break;

                default:
                    q = query.SortOrder == "desc" ? q.OrderByDescending(p => p.Name) : q.OrderBy(p => p.Name);
                    break;
            }

            // Total count BEFORE pagination
            int totalRecords = await q.CountAsync();

            // Pagination
            q = q.Skip((query.PageNumber - 1) * query.PageSize)
                 .Take(query.PageSize);

            var data = await q.ToListAsync();

            return (data, totalRecords);
        }


    }
}
