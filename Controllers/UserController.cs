using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ECommerceShopApi.Repositories.Role;
using ECommerceShopApi.Repositories.Account;

namespace ECommerceShopApi.Controllers {

    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRolesRepository _rolesRepository;
        private readonly IAccountRepository _accountRepository;

        public UserController(UserManager<ApplicationUser> userManager, IRolesRepository rolesRepository, IAccountRepository accountRepository) {

            _userManager = userManager;
            _rolesRepository = rolesRepository;
            _accountRepository = accountRepository;
        }


        
        [HttpPost("{username}/assign-role")]
        public async Task<IActionResult> AssignRoleToUser(string username, [FromBody] RoleModel model) {

            if (string.IsNullOrEmpty(model.Role)) {

                return BadRequest("سطح دسترسی الزامی است");
            }

            var user = await _accountRepository.GetUserByUsernameAsync(username);

            if (user == null) {

                return NotFound("کاربری یافت نشد");
            }

            var roleExists = await _rolesRepository.RoleExistsAsync(model.Role);

            if (!roleExists) {

                return BadRequest("سطح دسترسی با این نام وجود ندارد");
            }

            var result = await _rolesRepository.AddUserToRoleAsync(user, model.Role);

            if (result) {

                return Ok("سطح دسترسی با موفقیت تنظیم شد");
            } else {

                return StatusCode(500, "تنظیم سطح دسترسی با شکست مواجه شد");
            }
        }



        [HttpGet("{username}")]
        public async Task<IActionResult> FetchUserByUsername(string username) {

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) {

                return NotFound(new {
                    Message = "کاربر موردنظر یافت نشد"
                });
            }

            var userRole = await _rolesRepository.GetUserRoleAsync(user);

            return Ok(new {
                UserName = user.UserName,
                Role = userRole,
                FirstName = (user as ApplicationUser).FirstName,
                LastName = (user as ApplicationUser).LastName,
                Email = user.Email,
            });
        }


        
        [HttpGet("users")]
        public async Task<IActionResult> FetchAllUsers(int pageNumber = 1, int pageSize  = 25) {

            var skipCount = (pageNumber - 1) * pageSize;

            var users = await _userManager.Users
            .Skip(skipCount)
            .Take(pageSize)
            .ToListAsync();

            var usersWithRoles = new List<object>();

            foreach(var user in users) {

                var role = await _rolesRepository.GetUserRoleAsync(user);

                usersWithRoles.Add(new {
                    UserName = user.UserName,
                    Role = role,
                    FirstName = (user as ApplicationUser).FirstName,
                    LastName = (user as ApplicationUser).LastName,
                    Email = user.Email
                });
            }

            var totalUsers = await _userManager.Users.CountAsync();

            return Ok(new {
                TotalUsers = totalUsers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = Convert.ToInt32(Math.Ceiling(totalUsers / (double)pageSize)),
                Users = usersWithRoles
            });
        }
    }
}