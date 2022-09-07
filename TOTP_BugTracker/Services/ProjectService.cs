using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TOTP_BugTracker.Data;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Models.Enums;
using TOTP_BugTracker.Services.Interfaces;
using X.PagedList;

namespace TOTP_BugTracker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IRolesService _rolesService;
        public ProjectService(ApplicationDbContext context, IRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }

        public async Task<List<Project>> GetProjectsWithoutCompanyIdAsync()
        {
            List<Project> projects = await _context.Projects!
                                               .Where(p => p.Archived == false)
                                               .Include(p => p.Company)
                                               .Include(p => p.ProjectPriority)
                                               .ToListAsync();

            return projects;
        }
        public async Task<List<Project>> GetAllProjectsByCompanyIdAsync(int companyId)
        {
            var projects = await _context.Projects!
                                         .Where(p => p.Archived == false && p.CompanyId == companyId)
                                         .Include(p => p.Company)
                                         .Include(p => p.ProjectPriority)
                                         .ToListAsync();


            return projects;
        }
        public async Task<List<Project>> GetArchivedProjectsByCompanyIdAsync(int companyId)
        {
            var projects = await _context.Projects
                                         .Where(p => p.Archived && p.CompanyId == companyId)
                                         .Include(p => p.Company)
                                         .Include(p => p.ProjectPriority)
                                         .ToListAsync();

            return projects;
        }
        public async Task<List<Project>> GetArchivedProjectsWithoutCompanyIdAsync()
        {
            var projects = await _context.Projects
                                         .Where(p => p.Archived)
                                         .Include(p => p.Company)
                                         .Include(p => p.ProjectPriority)
                                         .ToListAsync();

            return projects;
        }
        public async Task AddProjectAsync(Project project)
        {
            _context.Add(project);
        }
        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            return project;
        }
        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
        }
        public async Task ArchiveProjectAsync(int projectId)
        {
            Project project = await GetProjectByIdAsync(projectId);

            if (project != null)
            {
                project.Archived = true;

                foreach (Ticket ticket in project.Tickets)
                {
                    ticket.ArchivedByProject = true;
                }
            }

            await _context.SaveChangesAsync();

        }
        public async Task RestoreProjectAsync(int projectId)
        {

            Project project = await GetProjectByIdAsync(projectId);

            if (project != null)
            {
                project.Archived = false;

                foreach (Ticket ticket in project.Tickets)
                {
                    ticket.ArchivedByProject = false;
                }

            }

        }

        public async Task<BTUser>? GetProjectManagerAsync(int projectId)
        {
            try
            {
                Project project = await GetProjectByIdAsync(projectId);

                foreach (BTUser member in project.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                    {
                        return member;
                    }

                }
                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            BTUser currentPM = await GetProjectManagerAsync(projectId)!;
            BTUser selectedPM = await _context.Users.FindAsync(userId);
            try
            {

                // Remove current project manager
                if (currentPM != null)
                {
                    await RemoveProjectManagerAsync(projectId);
                }


                try
                {
                    //Project? project = await GetProjectByIdAsync(projectId);
                    await AddUserToProjectAsync(selectedPM!, projectId);

                    return true;
                }
                catch (Exception)
                {

                    throw;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            try
            {
                Project? project = await GetProjectByIdAsync(projectId);

                foreach (BTUser member in project.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, nameof(BTRoles.ProjectManager)))
                    {
                        await RemoveUserFromProjectAsync(member, projectId);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RemoveUserFromProjectAsync(BTUser user, int projectId)
        {
            try
            {
                Project? project = await GetProjectByIdAsync(projectId);

                if (await IsUserOnProjectAsync(user.Id, projectId))
                {
                    project.Members.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {

            try
            {
                Project project = await GetProjectByIdAsync(projectId);

                if (project != null)
                {
                    bool result = project.Members.Any(m => m.Id == userId);
                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<bool> IsUserOnProjectAsync(int userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetUnassignedProjectsAsync(int companyId)
        {
            List<Project> rawProjects = await GetAllProjectsByCompanyIdAsync(companyId);

            List<Project> projects = new();

            foreach (Project project in rawProjects)
            {
                if (await GetProjectManagerAsync(project.Id)! == null)
                {
                    projects.Add(project);
                }
            }

            return projects;
        }

        public async Task<bool> AddUserToProjectAsync(BTUser user, int projectId)
        {

            Project project = await GetProjectByIdAsync(projectId);

            bool onProject = project.Members.Any(m => m.Id == user.Id);


            try
            {
                if (!onProject)
                {
                    project.Members.Add(user);

                    await _context.SaveChangesAsync();

                    return true;

                }



                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Task<bool> ProjectHasPMAsync(int projectId)
        {
            throw new NotImplementedException();
        }
    }

}

