using TOTP_BugTracker.Models;
using TOTP_BugTracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TOTP_BugTracker.Data;

namespace TOTP_BugTracker.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Company> GetCompanyInfoAsync(int? companyId)
        {
            try
            {
                Company? company = new();

                if (companyId == null)
                {
                    company = await _context.Companies
                                            .Include(c => c.Members)
                                            .Include(c => c.Projects)
                                            .Include(c => c.Invites)
                                            .FirstOrDefaultAsync(c => c.Id == companyId);
                }

                return company!;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<BTUser>> GetMembersAsync(int? companyId)
        {
            try
            {
                Company? company = await GetCompanyInfoAsync(companyId);

                if (company != null)
                {
                    List<BTUser> members = company.Members.ToList();

                    return members;
                }

                return null;



            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
