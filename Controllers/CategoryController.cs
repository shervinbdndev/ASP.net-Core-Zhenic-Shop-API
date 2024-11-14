using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceShopApi.Models.Category;
using ECommerceShopApi.Repositories.Category;

namespace ECommerceShopApi.Controllers {


    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase {

        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository) {

            _categoryRepository = categoryRepository;
        }


        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories() {

            var categories = await _categoryRepository.GetAllCategoriesAsync();

            return Ok(categories);
        }



        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategory(int id) {

            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null) return NotFound();

            return Ok(category);
        }



        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryModel category) {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _categoryRepository.AddCategoryAsync(category);

            return CreatedAtAction(nameof(GetCategory), new {id = category.Id}, category);
        }



        [HttpPut("edit/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryModel category) {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != category.Id) return BadRequest("مشخصه ناموجود");

            await _categoryRepository.UpdateCategoryAsync(category);

            return NoContent();
        }



        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id) {

            await _categoryRepository.DeleteCategoryAsync(id);

            return NoContent();
        }
    }
}