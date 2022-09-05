using TOTP_BugTracker.Models;
using X.PagedList;

namespace TOTP_BugTracker.Services.Interfaces
{
    public interface IProjectService
    {
        public Task<List<Project>> GetProjectsWithoutCompanyIdAsync();
        public Task<List<Project>> GetAllProjectsByCompanyIdAsync(int companyId);
        public Task<List<Project>> GetArchivedProjectsByCompanyIdAsync(int companyId);
        public Task<List<Project>> GetArchivedProjectsWithoutCompanyIdAsync();
        public Task AddProjectAsync(Project project);
        public Task<Project> GetProjectByIdAsync(int projectId);
        public Task UpdateProjectAsync(Project project);
        public Task ArchiveProjectAsync(int projectId);
        public Task RestoreProjectAsync(int projectId);

    }
}
