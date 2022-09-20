using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TOTP_BugTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int TicketId { get; set; }

        [Required]
        [DisplayName("Sender")]
        public string? SenderId { get; set; }

        [Required]
        [DisplayName("Recipient")]
        public string? RecipientId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and a maximum of {1} characters.", MinimumLength = 2)]
        public string? Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and a maximum of {1} characters.", MinimumLength = 2)]
        public string? Message { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        [DisplayName("Notification Type")]
        public int NotificationTypeId { get; set; }

        [DisplayName("Read")]
        public bool HasBeenViewed { get; set; }

        // Navigation Properties

        public virtual NotificationType? NotificationType { get; set; }
        public virtual Ticket? Ticket { get; set; }
        public virtual Project? Project { get; set; }
        public virtual BTUser? Sender { get; set; }
        public virtual BTUser? Recipient { get; set; }

    }
}
