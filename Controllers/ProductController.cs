using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerceShopApi.Repositories;


namespace ECommerceShopApi.Controllers {

    [Route("api/v1/[controller]")]
    [ApiController]    
    public class ProductController : ControllerBase {

        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository) {

            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts() {

            var products = await _productRepository.GetAllProductsAsync();

            return Ok(products);
        }


        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(int id) {

            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null) {

                return NotFound();
            }

            return Ok(product);
        }

        
        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] Product product) {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            await _productRepository.AddProductAsync(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }


        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product) {

            if (id != product.Id || !ModelState.IsValid) {

                return BadRequest();
            }

            await _productRepository.UpdateProductAsync(product);

            return NoContent();
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id) {

            await _productRepository.DeleteProductAsync(id);

            return NoContent();
        }
    }
}