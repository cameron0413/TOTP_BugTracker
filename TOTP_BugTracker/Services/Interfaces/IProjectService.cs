using TOTP_BugTracker.Models;
using X.PagedList;

namespace TOTP_BugTracker.Services.Interfaces
{
    public interface IProjectService
    {
        public Task AddProjectAsync(Project project);
        public Task<bool> AddProjectManagerAsync(string userId, int projectId);
        public Task<bool> AddUserToProjectAsync(BTUser user, int projectId);
        public Task ArchiveProjectAsync(int projectId);
        public Task<List<Project>> GetAllProjectsByCompanyIdAsync(int companyId);
        public Task<List<Project>> GetArchivedProjectsByCompanyIdAsync(int companyId);
        public Task<List<Project>> GetArchivedProjectsWithoutCompanyIdAsync();
        public Task<IEnumerable<Project>> GetIEnumOfAllProjectsByCompanyIdAsync(int companyId);
        public Task<BTUser>? GetProjectManagerAsync(int projectId);
        public Task<List<Project>> GetProjectsWithoutCompanyIdAsync();
        public Task<Project> GetProjectByIdAsync(int projectId);
        public Task<bool> IsUserOnProjectAsync(int userId, int projectId);
        public Task<bool> ProjectHasPMAsync(int projectId);
        public Task RemoveProjectManagerAsync(int projectId);
        public Task<bool> RemoveUserFromProjectAsync(BTUser user, int projectId);
        public Task RestoreProjectAsync(int projectId);
        public Task UpdateProjectAsync(Project project);
        public Task<List<Project>> GetUnassignedProjectsAsync(int companyId);

    }
}
