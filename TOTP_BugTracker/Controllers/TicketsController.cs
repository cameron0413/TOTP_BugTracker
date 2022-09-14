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
        private readonly IHistoryService _historyService;
        private readonly INotificationService _notificationService;

        public TicketsController(
                                 ApplicationDbContext context,
                                 UserManager<BTUser> userManager,
                                 IProjectService projectService,
                                 ITicketService ticketService,
                                 IRolesService rolesService,
                                 IHistoryService historyService,
                                 INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _projectService = projectService;
            _ticketService = ticketService;
            _rolesService = rolesService;
            _historyService = historyService;
            _notificationService = notificationService;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddTicketComment(int? id)
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






            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Description", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketTypeId"] = new SelectList(_context.Set<TicketType>(), "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketComment([Bind("Id,TicketId,Comment")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ticketComment.UserId = _userManager.GetUserId(User);
                    ticketComment.Created = DataUtility.GetPostgresDate(DateTime.Now);

                    await _ticketService.AddTicketCommentAsync(ticketComment);

                    await _historyService.AddHistoryAsync(ticketComment.TicketId, nameof(TicketComment), ticketComment.UserId);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }





        #region Archived Tickets
        public async Task<IActionResult> ArchivedTickets()
        {
            int companyId = (await _userManager.GetUserAsync(User)).CompanyId;

            List<Ticket> archivedTickets = await _ticketService.GetArchivedTicketsByCompanyIdAsync(companyId);
            return View(archivedTickets);
        } 
        #endregion




        #region GET method of AssignDev
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
        #endregion

        #region POST method of AssignDev
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignDev(AssignTicketDevViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DevId))
            {
                await _projectService.AddProjectManagerAsync(model.DevId, model.Ticket!.Id);
                return RedirectToAction(nameof(Index));

            }
            int companyId = User.Identity!.GetCompanyId();


            ModelState.AddModelError("DevID", "No Developer chosen! Please select a Developer.");


            Ticket? oldTicket = await _ticketService.GetTicketAsNoTrackingAsync(model.Ticket!.Id, companyId);

            //Get companyId

            model.Ticket = await _ticketService.GetTicketByIdAsync(model.Ticket!.Id);

            string? currentDevId = (await _ticketService.GetTicketDevAsync(model.Ticket.Id)!)?.Id;

            // Service Call to RoleService
            model.DevList = new SelectList(await _rolesService.GetUsersInRoleAsync(nameof(BTRoles.Developer), companyId), "Id", "FullName", currentDevId);

            await _projectService.AddProjectManagerAsync(model.DevId, model.Ticket.Id);

            // Add Ticket History
            BTUser btUser = await _userManager.GetUserAsync(User);
            Ticket? newTicket = await _ticketService.GetTicketAsNoTrackingAsync(model.Ticket!.Id, companyId);
            await _historyService.AddHistoryAsync(oldTicket, newTicket, btUser.Id);

            // Add Notification
            Notification notification = new()
            {
                NotificationTypeId = (await _context.NotificationTypes.FirstOrDefaultAsync(n => n.Name == nameof(BTNotificationTypes.Ticket)))!.Id,
                TicketId = model.Ticket.Id,
                Title = "New Ticket Added",
                Message = $"Ticket : {model.Ticket.Title}, was assigned by {btUser.FullName}",
                Created = DataUtility.GetPostgresDate(DateTime.Now),
                SenderId = btUser.Id,
                RecipientId = model.DevId
            };



            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AssignDev), new { id = model.Ticket!.Id });
        } 
        #endregion


        #region TICKETS INDEX
        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            int companyId = User.Identity!.GetCompanyId();

            List<Ticket> tickets = await _ticketService.GetAllTicketsByCompanyIdAsync(companyId);



            return View(tickets);
        } 
        #endregion

        #region DETAILS
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
        #endregion

        #region GET method of CREATE
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
        #endregion

        #region POST method of CREATE
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
                string userId = _userManager.GetUserId(User);
                // Add Ticket History
                Ticket newTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id, companyId);
                await _historyService.AddHistoryAsync(null!, newTicket, userId);

                // Add Ticket Notification
                BTUser btUser = await _userManager.GetUserAsync(User);
                BTUser projectmanager = await _projectService.GetProjectManagerAsync(ticket.ProjectId)!;
                Notification notification = new()
                {
                    NotificationTypeId = (await _context.NotificationTypes.FirstOrDefaultAsync(n => n.Name == nameof(BTNotificationTypes))).Id,
                    TicketId = ticket.Id,
                    Title = "New Ticket Added",
                    Message = $"New Ticket: {ticket.Title} was created by {ticket.SubmitterUser!.FullName}",
                    Created = DataUtility.GetPostgresDate(DateTime.Now),
                    SenderId = userId,
                    RecipientId = projectmanager?.Id
                };



                await _notificationService.AddNotificationAsync(notification);
                if (projectmanager != null)
                {
                    await _notificationService.SendEmailNotificationAsync(notification, $"New Ticket Added for Project: {ticket.Project!.Name}");
                }
                else
                {
                    notification.RecipientId = userId;
                    await _notificationService.SendEmailNotificationAsync(notification, $"New Ticket Added for Project: {ticket.Project!.Name}");
                }


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Name", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        } 
        #endregion

        #region GET method of EDIT
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






            ViewData["ProjectId"] = new SelectList(await _projectService.GetAllProjectsByCompanyIdAsync(companyId), "Id", "Description", ticket.ProjectId);
            ViewData["TicketPriorityId"] = new SelectList(_context.TicketPriorities, "Id", "Id", ticket.TicketPriorityId);
            ViewData["TicketTypeId"] = new SelectList(_context.Set<TicketType>(), "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        } 
        #endregion

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
                int companyid = User.Identity!.GetCompanyId();
                string userId = _userManager.GetUserId(User);
                Ticket? oldTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id, companyId);

                try
                {
                    ticket.Created = DataUtility.GetPostgresDate(ticket.Created);
                    ticket.Updated = DataUtility.GetPostgresDate(DateTime.Now);


                    await _ticketService.UpdateTicketAsync(ticket);

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

                // Add History
                Ticket newTicket = await _ticketService.GetTicketAsNoTrackingAsync(ticket.Id, companyid);
                await _historyService.AddHistoryAsync(oldTicket, newTicket, userId);

                // Add Notification


                await _context.SaveChangesAsync();
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
