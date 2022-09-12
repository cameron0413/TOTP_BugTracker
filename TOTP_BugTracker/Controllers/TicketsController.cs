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
using TOTP_BugTracker.Extensions;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Models.Enums;
using TOTP_BugTracker.Models.ViewModels;
using TOTP_BugTracker.Services.Interfaces;

namespace TOTP_BugTracker.Controllers
{

    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BTUser> _userManager;
        private readonly IProjectService _projectService;
        private readonly ITicketService _ticketService;
        private readonly IRolesService _rolesService;

        public TicketsController(
                                 ApplicationDbContext context,
                                 UserManager<BTUser> userManager,
                                 IProjectService projectService,
                                 ITicketService ticketService,
                                 IRolesService rolesService)
        {
            _context = context;
            _userManager = userManager;
            _projectService = projectService;
            _ticketService = ticketService;
            _rolesService = rolesService;
        }



        public async Task<IActionResult> ArchivedTickets()
        {
            int companyId = (await _userManager.GetUserAsync(User)).CompanyId;

            List<Ticket> archivedTickets = await _ticketService.GetArchivedTicketsByCompanyIdAsync(companyId);
            return View(archivedTickets);
        }




        [Authorize(Roles = "ProjectManager, Admin")]
        public async Task<IActionResult> AssignDev(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AssignTicketDevViewModel model = new();

            int companyId = (await _userManager.GetUserAsync(User)).CompanyId;

            model.Ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            string? currentDevId = (await _ticketService.GetTicketDevAsync(model.Ticket.Id)!)?.Id;

            // Service Call to RoleService
            model.DevList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.ProjectManager), companyId), "Id", "FullName", currentDevId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignDev(AssignTicketDevViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DevId))
            {
                await _projectService.AddProjectManagerAsync(model.DevId, model.Ticket!.Id);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("PMID", "No Project Manager chosen! Please select a PM.");


            //Get companyId
            int companyId = (await _userManager.GetUserAsync(User)).CompanyId;

            model.Ticket = await _ticketService.GetTicketByIdAsync(model.Ticket!.Id);

            string? currentDevId = (await _ticketService.GetTicketDevAsync(model.Ticket.Id)!)?.Id;

            // Service Call to RoleService
            model.DevList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), companyId), "Id", "FullName", currentDevId);

            await _projectService.AddProjectManagerAsync(model.DevId, model.Ticket.Id);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AssignDev), new { id = model.Ticket!.Id });
        }


        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            int companyId = User.Identity!.GetCompanyId();

            List<Ticket> tickets = await _ticketService.GetAllTicketsByCompanyIdAsync(companyId);



            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _ticketService.GetAllTicketsByCompanyIdAsync == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> CreateAsync()
        {
            int companyId = User.Identity!.GetCompanyId();
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Name");
            //ViewData["SubmitterUserId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name");
            //ViewData["TicketStatusId"] = new SelectList(_context.Set<TicketStatus>(), "Id", "Id");
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ProjectId,Description,TicketTypeId,TicketPriorityId,SubmitterUserId")] Ticket ticket)
        {
            int companyId = User.Identity!.GetCompanyId();

            ModelState.Remove("SubmitterUserId");

            if (ModelState.IsValid)
            {
                int statusId = (await _context.TicketStatuses.FirstOrDefaultAsync(s => s.Name == nameof(BTTicketStatuses.New)))!.Id;

                ticket.TicketStatusId = statusId;
                ticket.Created = DataUtility.GetPostgresDate(DateTime.Now);
                ticket.SubmitterUserId = _userManager.GetUserId(User);


                await _ticketService.AddTicketAsync(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Name", ticket.ProjectId);

            //ViewData["SubmitterUserId"] = new SelectList(_context.Users, "Id", "Name", ticket.SubmitterUserId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewData["TicketStatusId"] = new SelectList(_context.Set<TicketStatus>(), "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int companyId = User.Identity!.GetCompanyId();

            if (id == null || _ticketService.GetAllTicketsByCompanyIdAsync(companyId) == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "FullName", ticket.DeveloperUserId);
            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Description", ticket.ProjectId);
            //ViewData["SubmitterUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.SubmitterUserId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            //ViewData["TicketStatusId"] = new SelectList(_context.Set<TicketStatus>(), "Id", "Id", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.Set<TicketType>(), "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Created,Updated,Archived,ArchivedByProject,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,DeveloperUserId,SubmitterUserId")] Ticket ticket)
        {
            int companyId = User.Identity!.GetCompanyId();

            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _ticketService.UpdateTicketAsync(ticket);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            //ViewData["DeveloperUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.DeveloperUserId);
            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Description", ticket.ProjectId);
            //ViewData["SubmitterUserId"] = new SelectList(_context.Users, "Id", "Id", ticket.SubmitterUserId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Archive/5
        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);

            

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ticket'  is null.");
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket != null)
            {
                ticket.Archived = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Tickets/Restore/5
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id.Value);



            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ticket'  is null.");
            }

            Ticket ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket != null)
            {
                ticket.Archived = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        private bool TicketExists(int id)
        {
            return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        public async Task<IActionResult> UnassignedTickets(int projectId)
        {
            IEnumerable<Ticket> tickets = await _ticketService.GetUnassignedTicketsByProjectIdAsync(projectId);

            return View(tickets);
        }


    }
}
