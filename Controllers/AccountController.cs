using ECommerceShopApi.Utils;
using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerceShopApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using ECommerceShopApi.Repositories.Role;

namespace ECommerceShopApi.Controllers {
    

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {

        private readonly IAccountRepository _accountRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AccountController(IAccountRepository accountRepository, IRolesRepository rolesRepository, JwtTokenGenerator jwtTokenGenerator) {

            _accountRepository = accountRepository;
            _rolesRepository = rolesRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }




        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model) {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            var result = await _accountRepository.RegisterUserAsync(model);

            if (!result.Succeeded) {

                return BadRequest(result.Errors);
            }

            return Ok( new {
                    Message = "عملیات ثبت نام موفقیت آمیز بود"
                }
            );
        }

        

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            var loginResponse = await _accountRepository.LoginUserAsync(model);

            if (loginResponse == null) {

                return Unauthorized(new {
                        Message = "فرایند ورود به حساب کاربری با مشکل مواجه شد"
                    }
                );
            }

            return Ok(loginResponse);
        }



        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model) {

            if (!ModelState.IsValid) {

                return BadRequest("دیتای ورودی معتبر نیست");
            }

            var result = await _accountRepository.ResetPasswordAsync(model.Email, model.token, model.newPassword);

            if (result.Succeeded) {

                return Ok(new {
                    Message = "رمز عبور شما با موفقیت بازنشانی شد"
                });
            }

            return BadRequest("عملیات بازنشانی رمز عبور ناموفق بود");
        }


        

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut() {

            await _accountRepository.SignOutUserAsync();

            return Ok(new {
                    Message = "خروج از حساب کاربری با موفقیت انجام شد"
                }
            );
        }



        [Authorize(Roles = "Admin, Customer")]
        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> DeleteUserAsync(string username) {

            var (success, message) = await _accountRepository.DeleteUserIfAuthorizedAsync(User, username);
            
            return success ? Ok(message) : StatusCode(403, message);
        }
    }
}