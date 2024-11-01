using ECommerceShopApi.Utils;
using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerceShopApi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceShopApi.Controllers {
    

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {

        private readonly IAccountRepository _accountRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AccountController(IAccountRepository accountRepository, JwtTokenGenerator jwtTokenGenerator) {

            _accountRepository = accountRepository;
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

        

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut() {

            await _accountRepository.SignOutUserAsync();

            return Ok(new {
                    Message = "خروج از حساب کاربری با موفقیت انجام شد"
                }
            );
        }
    }
}