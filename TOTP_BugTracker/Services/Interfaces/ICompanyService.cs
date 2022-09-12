using TOTP_BugTracker.Models;

namespace TOTP_BugTracker.Services.Interfaces
{
    public interface ICompanyService
    {
        public Task<List<BTUser>> GetMembersAsync(int? companyId);
        public Task<Company> GetCompanyInfoAsync(int? companyId);
    }
}
