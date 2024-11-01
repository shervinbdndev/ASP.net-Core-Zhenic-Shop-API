using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceShopApi.Repositories {

    public interface IAccountRepository {

        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        Task<LoginResponse> LoginUserAsync(LoginModel model);
        Task SignOutUserAsync();
    }
}