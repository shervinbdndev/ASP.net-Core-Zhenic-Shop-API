using ECommerceShopApi.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceShopApi.Repositories.Role {

    public class RolesRepository : IRolesRepository {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesRepository(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) {

            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<bool> CreateRoleAsync(string roleName) {

            if (await RoleExistsAsync(roleName)) return false;

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

            return result.Succeeded;
        }


        public async Task<bool> RoleExistsAsync(string roleName) {

            return await _roleManager.RoleExistsAsync(roleName);
        }


        public async Task<bool> DeleteRoleAsync(string roleName) {

            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded;
        }


        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync() {

            return _roleManager.Roles.ToList();
        }


        public async Task<bool> AddUserToRoleAsync(ApplicationUser user, string roleName) {

            if (!await RoleExistsAsync(roleName)) return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);

            return result.Succeeded;
        }


        public async Task<bool> UpdateUserRoleAsync(ApplicationUser user, string newRole) {

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removalResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removalResult.Succeeded) return false;

            return await AddUserToRoleAsync(user, newRole);
        }


        public async Task<bool> RemoveUserFromRoleAsync(ApplicationUser user, string roleName) {

            if (!await RoleExistsAsync(roleName)) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            return result.Succeeded;
        }


        public async Task<bool> IsUserAdminAsync(ApplicationUser user) {

            return await _userManager.IsInRoleAsync(user, "Admin");
        }


        public async Task<string> GetUserRoleAsync(ApplicationUser user) {

            var roles = await _userManager.GetRolesAsync(user);

            return roles.FirstOrDefault();
        }
    }
}