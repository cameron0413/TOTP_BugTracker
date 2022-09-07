using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public TicketService(ApplicationDbContext context,
                             IRolesService rolesService,
                             IProjectService projectService)
        {
            _context = context;
            _rolesService = rolesService;
            _projectService = projectService;
        }
    }
}
