using ECommerceShopApi.Utils;
using System.Security.Claims;
using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Identity;
using ECommerceShopApi.Repositories.Role;

namespace ECommerceShopApi.Repositories {

    public class AccountRepository : IAccountRepository {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRolesRepository _rolesRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IRolesRepository rolesRepository, JwtTokenGenerator jwtTokenGenerator) {

            _userManager = userManager;
            _signInManager = signInManager;
            _rolesRepository = rolesRepository;
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

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) {

                await _rolesRepository.AddUserToRoleAsync(user, "Customer");
            }

            return result;
        }



        public async Task<(bool Success, string Message)> DeleteUserIfAuthorizedAsync(ClaimsPrincipal requestingUserClaims, string username) {

            var requestingUser = await GetUserByUsernameAsync(requestingUserClaims.FindFirstValue(ClaimTypes.Name));

            if (requestingUser == null) return (false, "Requesting user not found");

            var userToDelete = await GetUserByUsernameAsync(username);

            if (userToDelete == null) return (false, "User not found.");

            if (requestingUser.UserName != username && !await _rolesRepository.IsUserAdminAsync(requestingUser)) {

                return (false, "You do not have permission to delete this user");
            }

            var result = await _userManager.DeleteAsync(userToDelete);

            if (!result.Succeeded) {

                return (false, "An error occured while deleting the user");
            }

            return (true, "حساب کاربری با موفقیت حذف گردید");
        }



        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user) {

            return await _userManager.DeleteAsync(user);
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



        public async Task SignOutUserAsync() {

            await _signInManager.SignOutAsync();
        }
    }
}