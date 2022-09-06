using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TOTP_BugTracker.Data;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Models.Enums;
using TOTP_BugTracker.Models.ViewModels;
using TOTP_BugTracker.Services.Interfaces;
using X.PagedList;

namespace TOTP_BugTracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BTUser> _userManager;
        private readonly IProjectService _projectService;
        private readonly IRolesService _rolesService;

        public ProjectsController(ApplicationDbContext context,
                                  IImageService imageService,
                                  UserManager<BTUser> userManager,
                                  IProjectService projectService,
                                  IRolesService rolesService)
        {
            _context = context;
            _imageService = imageService;
            _userManager = userManager;
            _projectService = projectService;
            _rolesService = rolesService;

        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            List<Project> projects = await _context.Projects!
                                               .Where(p => p.Archived == false)
                                               .Include(p => p.Company)
                                               .Include(p => p.ProjectPriority)
                                               .ToListAsync();
            
            return View(projects);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignProjectManager(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            AssignPMViewModel model = new();

            int companyId = (await _userManager.GetUserAsync(User)).CompanyId;

            model.Project = await _projectService.GetProjectByIdAsync(id.Value);

            string? currentPMId = (await _projectService.GetProjectManagerAsync(model.Project.Id)!)?.Id;

            // Service Call to RoleService
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId),"Id","FullName", currentPMId);

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignProjectManager(AssignPMViewModel model)
        {
            if (!string.IsNullOrEmpty(model.PMID))
            {
                await _projectService.AddProjectManagerAsync(model.PMID, model.Project!.Id);

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("PMID", "No Project Manager chosen! Please select a PM.");


            //Get companyId
            int companyId = (await _userManager.GetUserAsync(User)).CompanyId;

            model.Project = await _projectService.GetProjectByIdAsync(model.Project!.Id);

            string? currentPMId = (await _projectService.GetProjectManagerAsync(model.Project.Id)!)?.Id;

            // Service Call to RoleService
            model.PMList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName", currentPMId);

            await _projectService.AddProjectManagerAsync(model.PMID, model.Project.Id);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AssignProjectManager), new { id = model.Project!.Id});
        }

        public async Task<IActionResult> ArchivedIndex()
        {
            List<Project> archivedProjects = await _context.Projects!.Where(p => p.Archived == true).Include(p => p.Company)
                                               .Include(p => p.ProjectPriority)
                                               .ToListAsync();

            return View(archivedProjects);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Company)
                .Include(p => p.ProjectPriority)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [Authorize(Roles = "Admin, ProjectManager")]
        // GET: Projects/Create
        public IActionResult Create()
        {

            // To Do: Abstract the use of _context


            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Name");
            return View(new Project());
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CompanyId,Name,Description,StartDate,EndDate,ProjectPriorityId,ImageData,ImageType,ImageFormFile")] Project project)
        {
            if (ModelState.IsValid)
            {

                // TO DO: Make companyId retrieval more efficient
                // get company id
                project.CompanyId = (await _userManager.GetUserAsync(User)).CompanyId;


                project.Created = DataUtility.GetPostgresDate(DateTime.Now);
                project.StartDate = DataUtility.GetPostgresDate(project.StartDate);
                project.EndDate = DataUtility.GetPostgresDate(project.EndDate);

                if (project.ImageFormFile != null)
                {
                    project.ImageData = await _imageService.ConvertFileToByteArrayAsync(project.ImageFormFile);
                    project.ImageType = project.ImageFormFile.ContentType;
                }


                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", project.CompanyId);
            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Name", project.ProjectPriorityId);
            return View(project);
        }
        [Authorize]
        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", project.CompanyId);
            ViewData["ProjectPriorityId"] = new SelectList(_context.ProjectPriorities, "Id", "Name", project.ProjectPriorityId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyId,Name,Description,StartDate,EndDate,ProjectPriorityId,ImageData,ImageType,ImageFormFile")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    project.Created = DateTime.SpecifyKind(project.Created, DateTimeKind.Utc);
                    project.StartDate = DataUtility.GetPostgresDate(project.StartDate);
                    project.EndDate = DataUtility.GetPostgresDate(project.EndDate);

                    if (project.ImageFormFile != null)
                    {
                        project.ImageData = await _imageService.ConvertFileToByteArrayAsync(project.ImageFormFile);
                        project.ImageType = project.ImageFormFile.ContentType;
                    }

                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", project.CompanyId);
            ViewData["ProjectPriorityId"] = new SelectList(_context.Set<ProjectPriority>(), "Id", "Name", project.ProjectPriorityId);
            return View(project);
        }

        // GET: Projects/Archive/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Company)
                .Include(p => p.ProjectPriority)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Project'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.Archived = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .Include(p => p.Company)
                .Include(p => p.ProjectPriority)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Project'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                project.Archived = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnassignedProjects(int companyId)
        {
            companyId = (await _userManager.GetUserAsync(User)).CompanyId;

            IEnumerable<Project> projects = await _projectService.GetUnassignedProjectsAsync(companyId);

            return View(projects);
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
