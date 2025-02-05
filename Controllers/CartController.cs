using ECommerceShopApi.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ECommerceShopApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using ECommerceShopApi.Models.CartNameSpace;

namespace ECommerceShopApi.Controllers {

    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase {

        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository) {

            _cartRepository = cartRepository;
        }

        

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;



        [HttpGet]
        public async Task<IActionResult> GetCart() {

            var userId = GetUserId();

            return Ok(await _cartRepository.GetUserCartAsync(userId) ?? new Cart { UserId = userId });
        }



        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItem) {

            await _cartRepository.AddItemToCartAsync(GetUserId(), cartItem.ProductId, cartItem.Quantity);

            return Ok(new {
                message = "Item added to cart."
            });
        }



        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] CartItemDto cartItem) {

            await _cartRepository.ModifyCartItemAsync(cartItemId, cartItem.Quantity);

            return Ok(new {
                message = "Cart item updated."
            });
        }



        [HttpDelete("delete/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId) {

            await _cartRepository.ModifyCartItemAsync(cartItemId, remove: true);

            return Ok(new {
                message = "Cart item removed."
            });
        }


        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart() {

            await _cartRepository.ClearCartAsync(GetUserId());

            return Ok(new {
                message = "Cart Cleared."
            });
        }
    }
}