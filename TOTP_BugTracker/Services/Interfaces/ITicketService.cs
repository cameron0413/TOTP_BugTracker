﻿using TOTP_BugTracker.Models;

namespace TOTP_BugTracker.Services.Interfaces
{
    public interface ITicketService
    {
        public Task AddTicketAsync(Ticket ticket);
        public Task<bool> AddTicketDevAsync(string userId, int ticketId);
        public Task<bool> AddUserToTicketAsync(BTUser user, int ticketId);
        public Task ArchiveTicketAsync(int ticketId);
        public Task<List<Ticket>> GetAllTicketsByCompanyIdAsync(int companyId);
        public Task<List<Ticket>> GetAllTicketsByProjectIdAsync(int projectId);
        public Task<List<Ticket>> GetArchivedTicketsByCompanyIdAsync(int companyId);
        public Task<List<Ticket>> GetArchivedTicketsByProjectIdAsync(int projectId);
        public Task<List<Ticket>> GetArchivedTicketsWithoutCompanyIdAsync();
        public Task<BTUser>? GetTicketDevAsync(int ticketId);
        public Task<List<Ticket>> GetTicketsWithoutCompanyIdAsync();
        public Task<Ticket> GetTicketByIdAsync(int ticketId);
        public Task<List<Ticket>> GetUnassignedTicketsByProjectIdAsync(int projectId);
        public Task<bool> IsUserOnTicketAsync(int userId, int ticketId);
        public Task RemoveTicketDevAsync(int ticketId);
        public Task<bool> RemoveUserFromTicketAsync(BTUser user, int ticketId);
        public Task RestoreTicketAsync(int ticketId);
        public Task<bool> TicketHasDevAsync(int ticketId);
        public Task UpdateTicketAsync(Ticket ticket);
    }
}
