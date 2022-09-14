using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TOTP_BugTracker.Data;
using TOTP_BugTracker.Extensions;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Models.ViewModels;
using TOTP_BugTracker.Services.Interfaces;

namespace TOTP_BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectService _projectService;
        private readonly IImageService _imageService;
        private readonly ITicketService _ticketService;
        private readonly UserManager<BTUser> _userManager;
        private readonly ICompanyService _companyService;

        public HomeController(ApplicationDbContext context,
                              ILogger<HomeController> logger,
                              IProjectService projectService,
                              IImageService imageService,
                              ITicketService ticketService,
                              UserManager<BTUser> userManager,
                              ICompanyService companyService)
        {
            _context = context;
            _logger = logger;
            _projectService = projectService;
            _imageService = imageService;
            _ticketService = ticketService;
            _userManager = userManager;
            _companyService = companyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Dashboard View
        public async Task<IActionResult> Dashboard()
        {
            int companyId = User.Identity!.GetCompanyId();


            DashboardViewModel model = new();

            model.Company = await _companyService.GetCompanyInfoAsync(companyId);
            model.Projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);
            model.Tickets = await _ticketService.GetAllTicketsByCompanyIdAsync(companyId);
            model.Members = _context.Users.Where(u => u.CompanyId == companyId).ToList();



            return View(model);
        }

        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}