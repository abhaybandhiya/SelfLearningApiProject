using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SelfLearningApiProject.Services;

namespace SelfLearningApiProject.Controllers
{
        [ApiController]
        // Yeh line batati hai: yeh controller V2 ke liye hai
        [ApiVersion("2.0")]
        // URL me version mandatory banaya
        [Route("api/v{version:apiVersion}/product")]
        public class ProductV2Controller : ControllerBase
        {
            private readonly IProductService _productService;

            public ProductV2Controller(IProductService productService)
            {
                _productService = productService;
            }

            // GET api/v2/product
            [HttpGet]
            public async Task<IActionResult> GetAllV2()
            {
                var products = await _productService.GetAllProductsAsync();

                // V2 ka NEW behavior
                return Ok(new
                {
                    ApiVersion = "v2",
                    TotalCount = products.Count(),
                    Message = "This response is from Product API V2",
                    Data = products
                });
            }
        }
}
