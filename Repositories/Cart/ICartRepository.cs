using ECommerceShopApi.Models.CartNameSpace;

namespace ECommerceShopApi.Repositories {

    public interface ICartRepository {

        Task<Cart?> GetUserCartAsync(string userId);
        Task AddItemToCartAsync(string userId, int productId, int quantity);
        Task ModifyCartItemAsync(int cartItemId, int? quantity = null, bool remove = false);
        Task ClearCartAsync(string userId);
    }
}