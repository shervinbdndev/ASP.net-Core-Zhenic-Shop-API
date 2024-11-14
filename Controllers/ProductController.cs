using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceShopApi.Models.ProductModel;
using ECommerceShopApi.Repositories.ProductNameSpace;


namespace ECommerceShopApi.Controllers {

    [Route("api/v1/[controller]")]
    [ApiController]  
    [Authorize]  
    public class ProductController : ControllerBase {

        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger) {

            _productRepository = productRepository;
            _logger = logger;
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts() {

            var products = await _productRepository.GetAllProductsAsync();

            return Ok(products);
        }



        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(int id) {

            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null) {

                return NotFound( new {
                    Message = "محصول یافت نشد"
                });
            }

            return Ok(product);
        }


        
        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] Product product) {

            if (!ModelState.IsValid) {

                return BadRequest(new {
                    Message = "اطلاعات ورودی نامعتبر است"
                });
            }

            if (product.Id != 0) {

                return BadRequest("فیلد کلیدی درحین ساخت، تنظیم نمیشود");
            }

            await _productRepository.AddProductAsync(product);
            _logger.LogInformation($"Product Created: {product.Name}");

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product) {

            if (id != product.Id || !ModelState.IsValid) {

                return BadRequest("دیتای ورودی نامعتبر است");
            }

            var productExists = await _productRepository.GetProductByIdAsync(id);

            if (productExists == null) {

                return NotFound("محصول موردنظر یافت نشد");
            }

            await _productRepository.UpdateProductAsync(product);
            _logger.LogInformation($"Product Updated: {product.Name}");

            return NoContent();
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id) {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null) {

                return NotFound("محصول مورد نظر یافت نشد");
            }

            await _productRepository.DeleteProductAsync(id);
            _logger.LogInformation($"Product Deleted: {id}");

            return NoContent();
        }
    }
}