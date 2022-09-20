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
        public ProjectService(ApplicationDbContext context, IRolesService rolesService, IImageService imageService)
        {
            _context = context;
            _rolesService = rolesService;
            _imageService = imageService;
        }

        #region Get Projects WITHOUT Company ID Async
        public async Task<List<Project>> GetProjectsWithoutCompanyIdAsync()
        {
            List<Project> projects = await _context.Projects!
                                               .Where(p => p.Archived == false)
                                               .Include(p => p.Company)
                                               .Include(p => p.ProjectPriority)
                                               .ToListAsync();

            return projects;
        }

        #endregion

        #region Get All Projects WITH Company ID
        public async Task<List<Project>> GetAllProjectsByCompanyIdAsync(int companyId)
        {
            var projects = await _context.Projects!
                                         .Where(p => p.Archived == false && p.CompanyId == companyId)
                                         .Include(p => p.Company)
                                         .Include(p => p.ProjectPriority)
                                         .ToListAsync();


            return projects;
        }

        #endregion

        #region Get IEnumerable of All Projects By Company ID - use for paged list I think?
        public async Task<IEnumerable<Project>> GetIEnumOfAllProjectsByCompanyIdAsync(int companyId)
        {
            IEnumerable<Project> projects = await _context.Projects!
                                         .Where(p => p.Archived == false && p.CompanyId == companyId)
                                         .Include(p => p.Company)
                                         .Include(p => p.ProjectPriority)
                                         .ToListAsync();


            return projects;
        }

        #endregion

        #region Get All Projects By Priority
        public async Task<List<Project>> GetAllProjectsByPriorityAsync(int companyId, string priority)
        {
            try
            {
                var projects = await _context.Projects!
                                             .Where(p => p.Archived == false && p.CompanyId == companyId && p.ProjectPriority!.Name == priority)
                                             .Include(p => p.Company)
                                             .Include(p => p.ProjectPriority)
                                             .ToListAsync();


                return projects;
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion

        #region Get Archived Projects WITH Company ID
        public async Task<List<Project>> GetArchivedProjectsByCompanyIdAsync(int companyId)
        {
            var projects = await _context.Projects
                                         .Where(p => p.Archived && p.CompanyId == companyId)
                                         .Include(p => p.Company)
                                         .Include(p => p.ProjectPriority)
                                         .ToListAsync();

            return projects;
        }

        #endregion

        #region Get Archived Projects WITHOUT Company Id
        public async Task<List<Project>> GetArchivedProjectsWithoutCompanyIdAsync()
        {
            var projects = await _context.Projects
                                         .Where(p => p.Archived)
                                         .Include(p => p.Company)
                                         .Include(p => p.ProjectPriority)
                                         .ToListAsync();

            return projects;
        }

        #endregion

        #region Add project to company
        public async Task AddProjectAsync(Project project)
        {
            _context.Add(project);
        }
        #endregion

        #region Get Project by ID
        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .Include(p => p.Tickets)
                                            .Include(p => p.Company)
                                            .Include(p => p.ProjectPriority)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            return project;
        }

        #endregion

        #region Update Project
        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
        }

        #endregion

        #region Archive Project
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

        #endregion

        #region Restore an Archived Project
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

        #endregion

        #region Get Project Manager of Project
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

        #endregion

        #region Get Project Members By Role Async
        public async Task<List<BTUser>> GetProjectMembersByRoleAsync(int projectId, string roleName)
        {
            try
            {
                Project? project = await _context.Projects.Include(p => p.Members)
                                                          .FirstOrDefaultAsync(p => p.Id == projectId);
                List<BTUser> members = new();
                foreach (BTUser btUser in project!.Members!)
                {
                    if (await _rolesService.IsUserInRoleAsync(btUser, roleName))
                    {
                        members.Add(btUser);
                    }
                }
                return members;
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion

        #region Add Project Manager to Project
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

        #endregion

        #region Remove Project Manager from Project
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

        #endregion

        #region Remove User from Project - returns Boolean
        public async Task<bool> RemoveUserFromProjectAsync(BTUser user, int projectId)
        {
            try
            {
                Project? project = await GetProjectByIdAsync(projectId);

                if (await IsUserOnProjectAsync(user.Id, projectId) == true)
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

        #endregion

        #region Is User On Project(userId, projectId) - returns boolean
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

        #endregion

        #region Get Unassigned Projects
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

        #endregion

        #region Get User Projects Async (all projects related to a certain user)
        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            try
            {
                List<Project>? projects = (await _context.Users
                                                         .Include(u => u.Projects)!
                                                            .ThenInclude(p => p.Company)
                                                         .Include(u => u.Projects)!
                                                            .ThenInclude(p => p.Members)
                                                         .Include(u => u.Projects)!
                                                            .ThenInclude(p => p.Tickets)
                                                         .Include(u => u.Projects)!
                                                            .ThenInclude(t => t.Tickets)
                                                                .ThenInclude(t => t.DeveloperUser)
                                                         .Include(u => u.Projects)!
                                                             .ThenInclude(t => t.Tickets)
                                                                 .ThenInclude(t => t.SubmitterUser)
                                                         .Include(u => u.Projects)!
                                                             .ThenInclude(t => t.Tickets)
                                                                 .ThenInclude(t => t.TicketPriority)
                                                         .Include(u => u.Projects)!
                                                             .ThenInclude(t => t.Tickets)
                                                                 .ThenInclude(t => t.TicketStatus)
                                                         .Include(u => u.Projects)!
                                                             .ThenInclude(t => t.Tickets)
                                                                 .ThenInclude(t => t.TicketType)
                                                         .FirstOrDefaultAsync(u => u.Id == userId))?.Projects!.ToList();
                return projects!;
            }
            catch (Exception)
            {
                throw;
            }
        } 
        #endregion

        #region Get Project Members In Role
        public async Task<List<BTUser>> GetProjectMembersInRoleAsync(int projectId, string roleName)
        {
            try
            {
                Project? project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == projectId);

                List<BTUser> members = new();
                foreach (BTUser bTUser in project!.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(bTUser, roleName))
                    {
                        members.Add(bTUser);
                    }
                }

                return members;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Add User To Project
        public async Task<bool> AddUserToProjectAsync(BTUser? user, int projectId)
        {
            Project project = await GetProjectByIdAsync(projectId);

            bool onProject = project.Members.Any(m => m.Id == user!.Id);

            try
            {
                if (!onProject)
                {
                    project.Members.Add(user!);

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

        #endregion

        #region Check whether Project has PM
        public Task<bool> ProjectHasPMAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}

