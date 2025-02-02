﻿using System.ComponentModel.DataAnnotations;

namespace TOTP_BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string? PropertyName { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        [Required]
        public string? UserId { get; set; }

        // Navigation Properties
        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? User { get; set; }

    }
}
