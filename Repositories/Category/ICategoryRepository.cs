using ECommerceShopApi.Models.Category;

namespace ECommerceShopApi.Repositories.Category {

    public interface ICategoryRepository {

        Task<IEnumerable<CategoryModel>> GetAllCategoriesAsync();
        Task<CategoryModel> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(CategoryModel category);
        Task UpdateCategoryAsync(CategoryModel category);
        Task DeleteCategoryAsync(int id);
    }
}