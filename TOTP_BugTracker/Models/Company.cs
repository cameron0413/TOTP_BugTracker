using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TOTP_BugTracker.Models
{
    [Authorize]
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Company Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and a maximum of {1} characters.", MinimumLength = 2)]
        public string? Name { get; set; }

        [DisplayName("Company Description")]
        public string? Description { get; set; }


        //Properties for storing image
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; } = "";

        //Property for passing file information from the form(html) to the post.
        //Also not saved in the database via [NotMapped] attribute
        [NotMapped]
        [DisplayName("Company Logo")]
        public virtual IFormFile? ImageFormFile { get; set; }

        // Navigation Properties

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public virtual ICollection<BTUser> Members { get; set; } = new HashSet<BTUser>();

        public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();


    }
}
