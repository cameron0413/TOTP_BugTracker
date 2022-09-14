using TOTP_BugTracker.Data;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Services.Interfaces;

namespace TOTP_BugTracker.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly ApplicationDbContext _context;

        public HistoryService(ApplicationDbContext context)
        {
            _context = context;
        }


        #region Add History Async (ticket, ticket, string)
        public async Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId)
        {
            try
            {
                // NEW TICKET HAS BEEN ADDED
                if (oldTicket == null && newTicket != null)
                {

                    TicketHistory ticketHistory = new()
                    {
                        TicketId = newTicket.Id,
                        PropertyName = string.Empty,
                        OldValue = string.Empty,
                        NewValue = string.Empty,
                        Created = DataUtility.GetPostgresDate(DateTime.Now),
                        UserId = userId,
                        Description = "New Ticket Added"
                    };

                    try
                    {
                        await _context.AddAsync(ticketHistory);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else
                {
                    if (oldTicket!.Title != newTicket!.Title)
                    {
                        TicketHistory ticketHistory = new()
                        {
                            TicketId = newTicket.Id,
                            PropertyName = "Title",
                            OldValue = oldTicket.Title,
                            NewValue = newTicket.Title,
                            Created = DataUtility.GetPostgresDate(DateTime.Now),
                            UserId = userId,
                            Description = "Ticket Title modified."
                        };
                        await _context.AddAsync(ticketHistory);
                    }

                    if (oldTicket.Description != newTicket.Description)
                    {
                        TicketHistory ticketHistory = new()
                        {
                            TicketId = newTicket.Id,
                            PropertyName = "Description",
                            OldValue = oldTicket.Description,
                            NewValue = newTicket.Description,
                            Created = DataUtility.GetPostgresDate(DateTime.Now),
                            UserId = userId,
                            Description = "Ticket Description modified."
                        };
                        await _context.AddAsync(ticketHistory);
                    }

                    if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
                    {
                        TicketHistory ticketHistory = new()
                        {
                            TicketId = newTicket.Id,
                            PropertyName = "TicketPriority",
                            OldValue = oldTicket.TicketPriority!.Name,
                            NewValue = newTicket.TicketPriority!.Name,
                            Created = DataUtility.GetPostgresDate(DateTime.Now),
                            UserId = userId,
                            Description = "Ticket Priority ID modified."
                        };
                        await _context.AddAsync(ticketHistory);
                    }

                    if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
                    {
                        TicketHistory ticketHistory = new()
                        {
                            TicketId = newTicket.Id,
                            PropertyName = "TicketStatus",
                            OldValue = oldTicket.TicketStatus!.Name,
                            NewValue = newTicket.TicketStatus!.Name,
                            Created = DataUtility.GetPostgresDate(DateTime.Now),
                            UserId = userId,
                            Description = "Ticket Status modified."
                        };
                        await _context.AddAsync(ticketHistory);
                    }
                    
                    if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
                    {
                        TicketHistory ticketHistory = new()
                        {
                            TicketId = newTicket.Id,
                            PropertyName = "TicketType",
                            OldValue = oldTicket.TicketType!.Name,
                            NewValue = newTicket.TicketType!.Name,
                            Created = DataUtility.GetPostgresDate(DateTime.Now),
                            UserId = userId,
                            Description = "Ticket Type modified."
                        };
                        await _context.AddAsync(ticketHistory);
                    }

                    if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
                    {
                        TicketHistory ticketHistory = new()
                        {
                            TicketId = newTicket.Id,
                            PropertyName = "DeveloperUser",
                            OldValue = oldTicket.DeveloperUser!.FullName ?? "Not Assigned",
                            NewValue = newTicket.DeveloperUser!.FullName,
                            Created = DataUtility.GetPostgresDate(DateTime.Now),
                            UserId = userId,
                            Description = "Ticket Developer modified."
                        };
                        await _context.AddAsync(ticketHistory);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add History Async (int, string, string)
        public async Task AddHistoryAsync(int ticketId, string model, string userId)
        {
            try
            {
                Ticket ticket = await _context.Tickets!.FindAsync(ticketId);

                string description = model.ToLower().Replace("ticket", "");
                description = $"New {description} added to ticket: {ticket!.Title}";

                TicketHistory ticketHistory = new()
                {
                    TicketId = ticket.Id,
                    PropertyName = model,
                    OldValue = string.Empty,
                    NewValue = string.Empty,
                    Created = DataUtility.GetPostgresDate(DateTime.Now),
                    UserId = userId,
                    Description = description
                };
                try
                {
                    await _context.AddAsync(ticketHistory);
                    await _context.SaveChangesAsync();
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

        #region Get Company Ticket Histories Async
        public Task<List<TicketHistory>> GetCompanyTicketsHistoriesAsync(int companyId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get Project Tickets' Histories Async
        public Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId, int companyId)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
