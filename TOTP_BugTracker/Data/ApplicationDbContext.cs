using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TOTP_BugTracker.Models;

namespace TOTP_BugTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<BTUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TOTP_BugTracker.Models.Company>? Company { get; set; }
        public DbSet<TOTP_BugTracker.Models.Invite>? Invite { get; set; }
        public DbSet<TOTP_BugTracker.Models.Notification>? Notification { get; set; }
        public DbSet<TOTP_BugTracker.Models.NotificationType>? NotificationType { get; set; }
        public DbSet<TOTP_BugTracker.Models.Project>? Project { get; set; }
        public DbSet<TOTP_BugTracker.Models.ProjectPriority>? ProjectPriority { get; set; }
        public DbSet<TOTP_BugTracker.Models.Ticket>? Ticket { get; set; }
        public DbSet<TOTP_BugTracker.Models.TicketAttachment>? TicketAttachment { get; set; }
        public DbSet<TOTP_BugTracker.Models.TicketComment>? TicketComment { get; set; }
        public DbSet<TOTP_BugTracker.Models.TicketHistory>? TicketHistory { get; set; }
        public DbSet<TOTP_BugTracker.Models.TicketPriority>? TicketPriority { get; set; }
        public DbSet<TOTP_BugTracker.Models.TicketStatus>? TicketStatus { get; set; }
        public DbSet<TOTP_BugTracker.Models.TicketType>? TicketType { get; set; }
        public virtual DbSet<Company>? Companies { get; set; } = default!;
        public virtual DbSet<Invite>? Invites { get; set; } = default!;
        public virtual DbSet<Notification>? Notifications { get; set; } = default!;
        public virtual DbSet<NotificationType>? NotificationTypes { get; set; } = default!;
        public virtual DbSet<Project>? Projects { get; set; } = default!;
        public virtual DbSet<ProjectPriority>? ProjectPriorities { get; set; } = default!;
        public virtual DbSet<Ticket>? Tickets { get; set; } = default!;
        public virtual DbSet<TicketAttachment>? TicketAttachments { get; set; } = default!;
        public virtual DbSet<TicketComment>? TicketComments { get; set; } = default!;
        public virtual DbSet<TicketHistory>? TicketHistories { get; set; } = default!;
        public virtual DbSet<TicketPriority>? TicketPriorities { get; set; } = default!;
        public virtual DbSet<TicketStatus>? TicketStatuses { get; set; } = default!;
        public virtual DbSet<TicketType>? TicketTypes { get; set; } = default!;
    }
}