using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddUserToRoleAsync(BTUser user, string roleName)
        {
            try
            {
                bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            try
            {
                List<IdentityRole> result = new();

                result = await _context.Roles.ToListAsync();
                return result;



            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<IEnumerable<string>> GetUserRolesAsync(BTUser user)
        {
            try
            {
                IEnumerable<string> result = await _userManager.GetRolesAsync(user);




                return result;

            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<bool> RemoveUserFromRoleAsync(BTUser user, string roleName)
        {
            try
            {
                bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveUserFromRolesAsync(BTUser user, IEnumerable<string> roleNames)
        {
            try
            {
                bool result = (await _userManager.RemoveFromRolesAsync(user, roleNames)).Succeeded;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
