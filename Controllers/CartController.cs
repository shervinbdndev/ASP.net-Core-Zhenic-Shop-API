using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceShopApi.Controllers {

    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) {

            _context = context;
            _userManager = userManager;
        }

        // دریافت اطلاعات سبد خرید

        // اضافه کردن محصول به عنوان آیتم به سبد خرید

        // ویرایش سبد خرید

        //حذف محصول از سبد خرید
    }
}