using ECommerceShopApi.Models.ProductModel;

namespace ECommerceShopApi.Repositories.ProductNameSpace {

    public interface IProductRepository {

        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int category);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}