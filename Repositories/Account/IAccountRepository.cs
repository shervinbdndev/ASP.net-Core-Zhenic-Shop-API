using System.Security.Claims;
using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Identity;
using ECommerceShopApi.Models.Account;

namespace ECommerceShopApi.Repositories.Account {

    public interface IAccountRepository {

        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        Task<LoginResponse> LoginUserAsync(LoginModel model);
        Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
        Task<(bool Success, string Message)> DeleteUserIfAuthorizedAsync(ClaimsPrincipal requestingUserClaims ,string username);
        Task SignOutUserAsync();
    }
}