using ECommerceShopApi.Utils;
using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceShopApi.Repositories {

    public class AccountRepository : IAccountRepository {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenGenerator jwtTokenGenerator) {

            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }




        public async Task<ApplicationUser> GetUserByUsernameAsync(string username) {

            return await _userManager.FindByNameAsync(username) ?? throw new InvalidOperationException("User not found");
        }


        

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model) {

            var user = new ApplicationUser {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            return await _userManager.CreateAsync(user, model.Password);
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

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new LoginResponse {
                Success = true,
                Message = "ورود به حساب کاربری با موفقیت انجام شد",
                Token = token,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }



        public async Task SignOutUserAsync() {

            await _signInManager.SignOutAsync();
        }
    }
}