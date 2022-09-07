using Microsoft.AspNetCore.Identity;
using TOTP_BugTracker.Data;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Services.Interfaces;

namespace TOTP_BugTracker.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BTUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RolesService(RoleManager<IdentityRole> roleManager,
                            UserManager<BTUser> userManager,
                            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<BTUser> GetUserAsync(string userId)
        {
            BTUser user = _context.Users.FirstOrDefault(u => u.Id == userId)!;

            return user!;
        }

        public async Task<string> GetUserIdAsync(BTUser user)
        {
            string userId = await _userManager.GetUserIdAsync(user);

            return userId;
        }

        public async Task<List<BTUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {

            try
            {
                List<BTUser> btUsers = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
                List<BTUser> results = btUsers.Where(u => u.CompanyId == companyId).ToList();

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsUserInRoleAsync(BTUser member, string roleName)
        {
            try
            {
                bool result = await _userManager.IsInRoleAsync(member, roleName);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
