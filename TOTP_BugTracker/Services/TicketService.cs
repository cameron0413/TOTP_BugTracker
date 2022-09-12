using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TOTP_BugTracker.Data;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Models.Enums;
using TOTP_BugTracker.Services.Interfaces;
using X.PagedList;

namespace TOTP_BugTracker.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IRolesService _rolesService;
        private readonly IProjectService _projectService;
        private readonly UserManager<BTUser> _userManager;

        public TicketService(ApplicationDbContext context,
                             IRolesService rolesService,
                             IProjectService projectService,
                             UserManager<BTUser> userManager)
        {
            _context = context;
            _rolesService = rolesService;
            _projectService = projectService;
            _userManager = userManager;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddTicketDevAsync(string userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUserToTicketAsync(BTUser user, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task ArchiveTicketAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId)
        {

            List<Ticket> tickets = await _context.Tickets!
                                                 .Where(t => !t.Archived && !t.ArchivedByProject && t.Project.CompanyId == companyId)
                                                 .Include(t => t.DeveloperUser)
                                                 .Include(t => t.Project)
                                                 .Include(t => t.SubmitterUser)
                                                 .Include(t => t.TicketPriority)
                                                 .Include(t => t.TicketStatus)
                                                 .Include(t => t.TicketType)
                                                 .ToListAsync();

            return tickets;


        }


        public async Task<List<Ticket>> GetAllTicketsByProjectIdAsync(int projectId)
        {
            List<Ticket> tickets = await _context.Tickets!
                                                 .Where(t => !t.Archived && !t.ArchivedByProject && t.ProjectId == projectId)
                                                 .Include(t => t.DeveloperUser)
                                                 .Include(t => t.Project)
                                                 .Include(t => t.SubmitterUser)
                                                 .Include(t => t.TicketPriority)
                                                 .Include(t => t.TicketStatus)
                                                 .Include(t => t.TicketType)
                                                 .ToListAsync();

            return tickets;
        }

        public async Task<List<Ticket>> GetArchivedTicketsByCompanyIdAsync(int companyId)
        {
            List<Ticket> archivedTickets = await _context.Tickets!.Where(t => t.Archived == true).Include(t => t.Project)
                                               .Include(t => t.TicketPriority)
                                               .ToListAsync();

            return archivedTickets;
        }

        public async Task<List<Ticket>> GetArchivedTicketsByProjectIdAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetArchivedTicketsWithoutCompanyIdAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            Ticket ticket = await _context.Tickets!
                                          .Include(t => t.DeveloperUser)
                                          .Include(t => t.Project)
                                          .Include(t => t.SubmitterUser)
                                          .Include(t => t.TicketPriority)
                                          .Include(t => t.TicketStatus)
                                          .Include(t => t.TicketType)
                                          .FirstOrDefaultAsync(t => t.Id == ticketId)!;


            return ticket!;
        }

        public async Task<BTUser>? GetTicketDevAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetTicketsWithoutCompanyIdAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ticket>> GetUnassignedTicketsByProjectIdAsync(int projectId)
        {
            List<Ticket> tickets = await _context.Tickets!
                                                 .Where(t => !t.Archived && !t.ArchivedByProject && t.DeveloperUser == null)
                                                 .Include(t => t.DeveloperUser)
                                                 .Include(t => t.Project)
                                                 .Include(t => t.SubmitterUser)
                                                 .Include(t => t.TicketPriority)
                                                 .Include(t => t.TicketStatus)
                                                 .Include(t => t.TicketType)
                                                 .ToListAsync();

            return tickets;
        }

        public async Task<bool> IsUserOnTicketAsync(int userId, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveTicketDevAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveUserFromTicketAsync(BTUser user, int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task RestoreTicketAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TicketHasDevAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Update(ticket);
        }
    }
}
