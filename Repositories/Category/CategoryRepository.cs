using ECommerceShopApi.Models;
using Microsoft.EntityFrameworkCore;
using ECommerceShopApi.Models.Category;

namespace ECommerceShopApi.Repositories.Category {

    public class CategoryRepository : ICategoryRepository {

        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) {

            _context = context;
        }


        public async Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync() {

            return await _context.Categories.ToListAsync();
        }


        public async Task<CategoryModel> GetCategoryByIdAsync(int id) {

            return await _context.Categories.FindAsync(id) ?? throw new InvalidOperationException("Category not found.");
        }


        public async Task AddCategoryAsync(CategoryModel category) {

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();
        }


        public async Task UpdateCategoryAsync(CategoryModel category) {

            _context.Categories.Update(category);

            await _context.SaveChangesAsync();
        }


        public async Task DeleteCategoryAsync(int id) {

            var category = await GetCategoryByIdAsync(id);

            if (category != null) {

                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
            }
        }
    }
}