using ECommerceShopApi.Utils;
using System.Security.Claims;
using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Identity;
using ECommerceShopApi.Models.Account;
using ECommerceShopApi.Repositories.Role;

namespace ECommerceShopApi.Repositories.Account {

    public class AccountRepository : IAccountRepository {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRolesRepository _rolesRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly UserLastLogin _userLastLogin;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IRolesRepository rolesRepository, JwtTokenGenerator jwtTokenGenerator, UserLastLogin userLastLogin, ILogger<AccountRepository> logger) {

            _userManager = userManager;
            _signInManager = signInManager;
            _rolesRepository = rolesRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userLastLogin = userLastLogin;
            _logger = logger;
        }




        public async Task<ApplicationUser> GetUserByUsernameAsync(string username) {

            return await _userManager.FindByNameAsync(username) ?? throw new InvalidOperationException("User not found");
        }   



        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model) {

            var user = new ApplicationUser {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) {

                await _rolesRepository.AddUserToRoleAsync(user, "Customer");
            }

            return result;
        }



        public async Task<(bool Success, string Message)> DeleteUserIfAuthorizedAsync(ClaimsPrincipal requestingUserClaims, string username) {

            var usernameClaim = requestingUserClaims?.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(usernameClaim)) {

                return (false, "کاربر موردنظر احراز حویت نشده یا نام کاربری وجود ندارد");
            }

            var requestingUser = await GetUserByUsernameAsync(usernameClaim);

            if (requestingUser == null) return (false, "کاربر موردنظر یافت نشد");

            var userToDelete = await GetUserByUsernameAsync(username);

            if (userToDelete == null) return (false, "کاربری یافت نشد");

            if (requestingUser.UserName != username && !await _rolesRepository.IsUserAdminAsync(requestingUser)) {

                return (false, "شما اجازه حذف این کاربر را ندارید");
            }

            var result = await _userManager.DeleteAsync(userToDelete);

            if (!result.Succeeded) {

                return (false, "حذف کاربر با مشکل مواجه شد");
            }

            return (true, "حساب کاربری با موفقیت حذف گردید");
        }



        public async Task<LoginResponse> LoginUserAsync(LoginModel model) {
            
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded) {

                return new LoginResponse {
                    Success = false,
                    Message = "ورود ناموفق"
                };
            };

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null) throw new InvalidOperationException("User not found");

            await _userLastLogin.Set(user);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginResponse {
                Success = true,
                Message = "ورود به حساب کاربری با موفقیت انجام شد",
                Token = token,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }



        public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword) {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) {

                _logger.LogWarning($"تلاش برای بازنشانی رمز عبور برای ایمیل {email} ناموفق بود");

                return IdentityResult.Failed(new IdentityError {
                    Description = "کاربری یافت نشد"
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded) {

                _logger.LogInformation($"رمز عبور جدید برای ایمیل {email} تنظیم شد");
            } else {

                foreach (var error in result.Errors) {

                    _logger.LogWarning(error.Description);
                }
            }

            return result;
        }



        public async Task SignOutUserAsync() {

            await _signInManager.SignOutAsync();
        }
    }
}