using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceShopApi.Utils {

    public class UserLastLogin {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserLastLogin> _logger;

        public UserLastLogin(UserManager<ApplicationUser> userManager, ILogger<UserLastLogin> logger) {

            _userManager = userManager;
            _logger = logger;
        }


        public async Task Set(ApplicationUser user) {

            user.LastLogin = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) _logger.LogWarning($"Failed to update LastLogin for user {user.UserName}");
        }
    }
}