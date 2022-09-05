using Microsoft.EntityFrameworkCore;
using TOTP_BugTracker.Data;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Services.Interfaces;
using X.PagedList;

namespace TOTP_BugTracker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
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
            Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

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

    }

}

