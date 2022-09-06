using TOTP_BugTracker.Models;

namespace TOTP_BugTracker.Services.Interfaces
{
    public interface IRolesService
    {
        public Task<List<BTUser>> GetUsersInRoleAsync(string roleName, int companyId);
        public Task<bool> IsUserInRoleAsync(BTUser member, string roleName);
    }
}
