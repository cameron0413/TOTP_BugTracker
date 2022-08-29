using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TOTP_BugTracker.Models
{
    public class TicketAttachment
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Created { get; set; }
        public int? TicketId { get; set; }

        [Required]
        public string? UserId { get; set; }

        //Properties for storing image
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; } = "";

        //Property for passing file information from the form(html) to the post.
        //Also not saved in the database via [NotMapped] attribute
        [NotMapped]
        public virtual IFormFile? ImageFormFile { get; set; }

        // Navigation Properties

        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? User { get; set; }
    }
}
