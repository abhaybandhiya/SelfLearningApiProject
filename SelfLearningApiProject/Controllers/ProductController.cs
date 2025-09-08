using Microsoft.AspNetCore.Mvc;
using SelfLearningApiProject.Helpers;
using SelfLearningApiProject.Models.DTO;
using SelfLearningApiProject.Services;

namespace SelfLearningApiProject.Controllers
{
    // Yeh class ek API controller hai – Isme HTTP endpoints hote hain
    [ApiController] // Batata hai ki yeh controller automatic model validation karega
    [Route("api/[controller]")] // Yeh URL define karta hai: api/product (based on class name)

    public class ProductController : ControllerBase
    {
        // Service layer ka reference – business logic iske andar hoti hai
        private readonly IProductService _productService;

        // Logger ka reference – errors ya info log karne ke liye use hota hai
        private readonly ILogger<ProductController> _logger;

        // Constructor injection – DI system se IProductService ka object milega
        public ProductController(ILogger<ProductController> logger , IProductService productService) // IProductService ko inject karte hain, jo service layer ka interface hai
        {
            _logger = logger; // Logger ko initialize karte hain, jisse ki hum logs likh sakein
            _productService = productService; // Service ko initialize karte hain. Yeh service layer ke methods ko call karne ke liye use hoga
        }

        // HTTP GET method – sabhi products ko return karta hai
        // Route: GET api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll called with All products:"); // Info log

            // Service layer ko call kar rahe hain to get all products (DTO format me)
            var products = await _productService.GetAllProductsAsync();

            // 200 OK response ke saath products return kar rahe hain
            return Ok(products);
        }


        // HTTP GET method – specific product ko ID ke basis pe laata hai
        // Route: GET api/product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
             _logger.LogInformation($"GetById called with ID: {id}"); // ✅ Info log
            try
            {
                // Service layer ko call karte hain specific product ke liye
                var product = await _productService.GetProductByIdAsync(id);

                // Agar product nahi mila, to 404 Not Found return karo
                if (product == null)
                    return NotFound($"Product with ID {id} not found."); // 404 Not Found

                // Agar product mila, to 200 OK ke saath return karo
                return Ok(product); // 200 OK
            }
            catch (Exception ex)
            {
                // 500 Internal Server Error
                return StatusCode(500, $"Internal server error in GetById: {ex.Message}");
            }
        }

        // HTTP POST method – naya product create karega
        [HttpPost]
        
        public async Task<IActionResult> Create([FromBody] ProductDto productDto) // [FromBody] se batata hai ki data request body se aayega
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // ⬅️ Validation fail ho to ye chalega
            }
            // Agar client ne null bhej diya to 400 BadRequest return karo
            if (productDto == null)
                return BadRequest();

            // Service layer ko call karte hain naya product banane ke liye
            var createdProduct = await _productService.CreateProductAsync(productDto); // Yeh method naya product create karega aur uska DTO return karega

            var response = new ApiResponse<ProductDto>("Product created successfully", createdProduct);

            // 201 Created return karte hain (standard for POST)
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, response); // CreatedAtAction se batata hai ki naya resource ka URL kya hoga
            
        }

        // HTTP PUT method – existing product ko update karega
        [HttpPut("{id}")] 
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto) // [FromBody] se batata hai ki data request body se aayega
        {
            // Agar client ne null bhej diya to 400 BadRequest return karo
            if (productDto == null)
                return BadRequest();

            var success = await _productService.UpdateProductAsync(id, productDto); // Yeh method product ko update karega. agar update successful hua to true, nahi hua to false

            // Agar update successful nahi hua (matlab product ID se match nahi hua), to 404 Not Found return karo
            if (!success)
            {
                return NotFound(new ApiResponse<ProductDto>("Product not updated", productDto));
            }
            return Ok(new ApiResponse<ProductDto>("Product updated successfullyyy", productDto));
        }

        // HTTP DELETE method – product ko delete karega by ID
        [HttpDelete("{id}")]

        // Route: DELETE api/product/5 TO 5 id vale product ko delete karega // Deletes a product by ID 
        public async Task<IActionResult> Delete(int id, ProductDto productDto)  
        {
            // Service layer ko call karte hain product delete karne ke liye
            var success = await _productService.DeleteProductAsync(id);

            // Agar delete successful nahi hua (matlab product ID se match nahi hua), to 404 Not Found return karo
            if (!success)
                return NotFound(new ApiResponse<ProductDto>("No data deleted", productDto));
            // Agar delete successful hua, to 204 No Content return karo (matlab delete ho gaya)
            return Ok(new ApiResponse<ProductDto>("Product deleted successfully", productDto));
        }
        // Aap yahan aur bhi HTTP methods (POST, PUT, DELETE) add kar sakte hain jaise ki products create/update/delete karne ke liye
    }
}
