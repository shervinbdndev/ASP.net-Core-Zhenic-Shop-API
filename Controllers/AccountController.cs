using System.Text;
using System.Security.Claims;
using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerceShopApi.Controllers {
    

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {

            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model) {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            var user = new ApplicationUser {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {

                return BadRequest(result.Errors);
            }

            return Ok(
                new {
                    Message = "عملیات ثبت نام موفقیت آمیز بود"
                }
            );
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded) {

                return Unauthorized(
                    new {
                        Message = "فرایند ورود به حساب کاربری با مشکل مواجه شد"
                    }
                );
            }

            var user = await _userManager.FindByNameAsync(model.UserName) as ApplicationUser;

            if (user == null) {

                return Unauthorized(
                    new {
                        Message = "کاربری پیدا نشد"
                    }
                );
            }

            var token = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    issuer: "https://Zhenicshop.com",
                    audience: "https://Zhenicshop.com/api",
                    expires: DateTime.Now.AddDays(1),
                    claims: new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("FirstName", user.FirstName),
                        new Claim("LastName", user.LastName)
                    },
                    signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("in-kelide-man-ast-1321464-be-in-adad-tavajoh-nakonid")),
                        algorithm: SecurityAlgorithms.HmacSha256Signature
                    )
                )
            );

            return Ok(
                new {
                    Token = token,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }
            );
        }

        
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut() {

            await _signInManager.SignOutAsync();

            return Ok(
                new {
                    Message = "خروج از حساب کاربری با موفقیت انجام شد"
                }
            );
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