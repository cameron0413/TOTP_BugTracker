using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TOTP_BugTracker.Extensions;

namespace TOTP_BugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        public int? TicketId { get; set; }

        [Required]
        public string? UserId { get; set; }

        //Properties for storing image
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; } = "";

        //Property for passing file information from the form(html) to the post.
        //Also not saved in the database via [NotMapped] attribute
        [NotMapped]
        [DisplayName("Select a file")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf" })]
        public virtual IFormFile? ImageFormFile { get; set; }

        // Navigation Properties

        public virtual Ticket? Ticket { get; set; }
        public virtual BTUser? User { get; set; }
    }
}
