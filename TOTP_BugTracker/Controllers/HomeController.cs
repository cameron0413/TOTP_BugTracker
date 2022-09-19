using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TOTP_BugTracker.Data;
using TOTP_BugTracker.Extensions;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Models.ChartModels;
using TOTP_BugTracker.Models.Enums;
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


        #region Plotly Bar Data
        [HttpPost]
        public async Task<JsonResult> PlotlyBarChart()
        {
            PlotlyBarData plotlyData = new();
            List<PlotlyBar> barData = new();

            int companyId = User.Identity.GetCompanyId();

            List<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);

            //Bar One
            PlotlyBar barOne = new()
            {
                X = projects.Select(p => p.Name).ToArray(),
                Y = projects.SelectMany(p => p.Tickets).GroupBy(t => t.ProjectId).Select(g => g.Count()).ToArray(),
                Name = "Tickets",
                Type = "bar"
            };

            //Bar Two
            PlotlyBar barTwo = new()
            {
                X = projects.Select(p => p.Name).ToArray(),
                Y = projects.Select(async p => (await _projectService.GetProjectMembersByRoleAsync(p.Id, nameof(BTRoles.Developer))).Count).Select(c => c.Result).ToArray(),
                Name = "Developers",
                Type = "bar"
            };

            barData.Add(barOne);
            barData.Add(barTwo);

            plotlyData.Data = barData;

            return Json(plotlyData);
        }
        #endregion


        #region Google Project Tickets
        [HttpPost]
        public async Task<JsonResult> GglProjectTickets()
        {
            int companyId = User.Identity.GetCompanyId();

            List<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);

            List<object> chartData = new();
            chartData.Add(new object[] { "ProjectName", "TicketCount" });

            foreach (Project prj in projects)
            {
                chartData.Add(new object[] { prj.Name!, prj.Tickets.Count() });
            }

            return Json(chartData);
        } 
        #endregion

        #region Google Project Priority
        [HttpPost]
        public async Task<JsonResult> GglProjectPriority()
        {
            int companyId = User.Identity.GetCompanyId();

            List<Project> projects = await _projectService.GetAllProjectsByCompanyIdAsync(companyId);

            List<object> chartData = new();
            chartData.Add(new object[] { "Priority", "Count" });


            foreach (string priority in Enum.GetNames(typeof(BTProjectPriorities)))
            {
                int priorityCount = (await _projectService.GetAllProjectsByPriorityAsync(companyId, priority)).Count();
                chartData.Add(new object[] { priority, priorityCount });
            }

            return Json(chartData);
        }
        #endregion


        [HttpPost]
        public async Task<JsonResult> AmCharts()
        {

            AmChartData amChartData = new();
            List<AmItem> amItems = new();

            int companyId = User.Identity.GetCompanyId();

            List<Project> projects = (await _projectService.GetAllProjectsByCompanyIdAsync(companyId)).Where(p => p.Archived == false).ToList();

            foreach (Project project in projects)
            {
                AmItem item = new();

                item.Project = project.Name!;
                item.Tickets = project.Tickets.Count;
                item.Developers = (await _projectService.GetProjectMembersByRoleAsync(project.Id, nameof(BTRoles.Developer))).Count();

                amItems.Add(item);
            }

            amChartData.Data = amItems.ToArray();


            return Json(amChartData.Data);
        }


        public IActionResult Landing()
        {
            return View();
        }

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