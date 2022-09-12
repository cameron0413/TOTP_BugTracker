using Microsoft.AspNetCore.Mvc.Rendering;

namespace TOTP_BugTracker.Models.ViewModels
{
    public class ProjectMembersViewModel
    {
        public Project? Project { get; set; }
        public SelectList? MemberList { get; set; }
        public string? MemberId { get; set; }
    }
}
