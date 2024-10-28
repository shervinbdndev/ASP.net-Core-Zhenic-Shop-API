using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace ECommerceShopApi.Controllers {

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager) {

            _userManager = userManager;
        }


        [HttpGet("{username}")]
        public async Task<IActionResult> FetchUserByUsername(string username) {

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) {

                return NotFound(
                    new {
                        Message = "کاربر موردنظر یافت نشد"
                    }
                );
            }

            return Ok(new {
                UserName = user.UserName,
                FirstName = (user as ApplicationUser).FirstName,
                LastName = (user as ApplicationUser).LastName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
            });
        }

        
        [HttpGet("users")]
        public IActionResult FetchAllUsers() {

            var users = _userManager.Users.Select(user => new {
                UserName = user.UserName,
                FirstName = (user as ApplicationUser).FirstName,
                LastName = (user as ApplicationUser).LastName,
                Email = user.Email,
            }).ToList();

            return Ok(users);
        }
    }
}