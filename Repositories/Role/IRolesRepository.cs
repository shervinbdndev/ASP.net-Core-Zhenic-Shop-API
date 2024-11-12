using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceShopApi.Repositories.Role {

    public interface IRolesRepository {

        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<bool> UpdateUserRoleAsync(ApplicationUser user, string newRole);
        Task<bool> RemoveUserFromRoleAsync(ApplicationUser user, string roleName);
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<bool> IsUserAdminAsync(ApplicationUser user);
        Task<string> GetUserRoleAsync(ApplicationUser user);
    }
}