using ECommerceShopApi.Models;
using Microsoft.EntityFrameworkCore;
using ECommerceShopApi.Models.CartNameSpace;

namespace ECommerceShopApi.Repositories {

    public class CartRepository: ICartRepository {

        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) {

            _context = context;
        }


        public async Task<Cart?> GetUserCartAsync(string userId) {

            return await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task AddItemToCartAsync(string userId, int productId, int quantity) {

            var cart = await GetUserCartAsync(userId);

            if (cart == null) {

                cart = new Cart { UserId = userId };
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null) {

                existingItem.Quantity += quantity;
                _context.CartItems.Update(existingItem);
            } else {

                var newItem = new CartItem {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };

                await _context.CartItems.AddAsync(newItem);
            }

            await _context.SaveChangesAsync();
        }


        public async Task ModifyCartItemAsync(int cartItemId, int? quantity = null, bool remove = false) {

            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if (cartItem == null) return;

            if (remove) _context.CartItems.Remove(cartItem);

            else if (quantity.HasValue) cartItem.Quantity = quantity.Value;

            await _context.SaveChangesAsync();
        }


        public async Task ClearCartAsync(string userId) {

            var cart = await GetUserCartAsync(userId);

            if (cart?.Items.Any() == true) {

                _context.CartItems.RemoveRange(cart.Items);
                await _context.SaveChangesAsync();
            }
        }
    }
}