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
            try
            {
                await _context.AddAsync(ticket);

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddTicketAttachmentAsync(TicketAttachment ticketAttachment)
        {
            try
            {
                await _context.AddAsync(ticketAttachment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddTicketCommentAsync(TicketComment comment, int ticketId)
        {
            try
            {
                Ticket ticket = await GetTicketByIdAsync(ticketId);

                await _context.AddAsync(comment);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId)
        {

            List<Ticket> tickets = await _context.Tickets!
                                                 .Where(t => !t.Archived && !t.ArchivedByProject && t.Project!.CompanyId == companyId)
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

        public async Task<List<Ticket>> GetAllTicketsRelatedToUserAsync(string userId)
        {
            try
            {
                List<Ticket> relatedTickets = await _context.Tickets.Where(t => t.DeveloperUserId == userId ||
                                                                           t.SubmitterUserId == userId ||
                                                                           t.Project!.Members.Any(m => m.Id == userId))
                                                                    .Include(t => t.DeveloperUser)
                                                                    .Include(t => t.Project)
                                                                    .Include(t => t.SubmitterUser)
                                                                    .Include(t => t.TicketPriority)
                                                                    .Include(t => t.TicketStatus)
                                                                    .Include(t => t.TicketType)
                                                                    .ToListAsync();

                return relatedTickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetArchivedTicketsByCompanyIdAsync(int companyId)
        {

            List<Ticket> archivedTickets = await _context.Tickets!.Where(t => t.Archived == true || t.ArchivedByProject == true && t.Project!.CompanyId == companyId).Include(t => t.Project)
                                               .Include(t => t.TicketPriority)
                                               .ToListAsync();

            return archivedTickets;
        }

        public async Task<List<Ticket>> GetArchivedTicketsByProjectIdAsync(int projectId)
        {
            try
            {
                List<Ticket> archivedTicketsFromProject = await _context.Tickets.Where(t => t.Archived == true || t.ArchivedByProject == true && t.ProjectId == projectId)
                                                                                .Include(t => t.Project)
                                                                                .Include(t => t.DeveloperUser)
                                                                                .Include(t => t.Project)
                                                                                .Include(t => t.SubmitterUser)
                                                                                .Include(t => t.TicketPriority)
                                                                                .Include(t => t.TicketStatus)
                                                                                .Include(t => t.TicketType)
                                                                                .ToListAsync();

                return archivedTicketsFromProject;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetArchivedTicketsWithoutCompanyIdAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket> GetTicketAsNoTrackingAsync(int ticketId, int companyId)
        {
            try
            {
                Ticket? ticket = await _context.Projects!
                                               .Where(p => p.CompanyId == companyId && p.Archived == false)
                                               .SelectMany(p => p.Tickets)
                                                  .Include(t => t.DeveloperUser)
                                                  .Include(t => t.Attachments)
                                                  .Include(t => t.Project)
                                                  .Include(t => t.History)
                                                  .Include(t => t.Comments)
                                                  .Include(t => t.SubmitterUser)
                                                  .Include(t => t.TicketPriority)
                                                  .Include(t => t.TicketStatus)
                                                  .Include(t => t.TicketType)
                                                  .Where(t => t.Archived == false && t.ArchivedByProject == false)
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(t => t.Id == ticketId);


                return ticket!;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            Ticket? ticket = await _context.Tickets!
                                           .Include(t => t.DeveloperUser)
                                           .Include(t => t.Attachments)
                                           .Include(t => t.Project)
                                           .Include(t => t.History)
                                           .Include(t => t.Comments)
                                              .ThenInclude(c => c.User)
                                           .Include(t => t.SubmitterUser)
                                           .Include(t => t.TicketPriority)
                                           .Include(t => t.TicketStatus)
                                           .Include(t => t.TicketType)
                                           .FirstOrDefaultAsync(t => t.Id == ticketId)!;


            return ticket!;
        }

        public async Task<BTUser>? GetTicketDevAsync(int ticketId)
        {
            Ticket ticket = await GetTicketByIdAsync(ticketId);

            BTUser developer = ticket.DeveloperUser!;

            return developer;
        }

        public async Task<List<Ticket>> GetTicketsWithoutCompanyIdAsync()
        {
            List<Ticket> allTickets = await _context.Tickets.Where(t => t.Archived == false && t.ArchivedByProject == false).ToListAsync();


            return allTickets;
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
