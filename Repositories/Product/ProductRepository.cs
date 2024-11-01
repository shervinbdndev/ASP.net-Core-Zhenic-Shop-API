using ECommerceShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceShopApi.Repositories {

    public class ProductRepository : IProductRepository {

        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() {

            return await _context.Products.ToListAsync();
        }



        public async Task<Product> GetProductByIdAsync(int id) {

            return await _context.Products.FindAsync(id);
        }



        public async Task<bool> ProductExistsAsync(int id) {

            return await _context.Products.AnyAsync(p => p.Id == id);
        }



        public async Task AddProductAsync(Product product) {

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }



        public async Task UpdateProductAsync(Product product) {

            if (await ProductExistsAsync(product.Id)) {

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }


        
        public async Task DeleteProductAsync(int id) {

            var product = await GetProductByIdAsync(id);

            if (product != null) {

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}