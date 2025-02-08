using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TOTP_BugTracker.Services.Interfaces;
using TOTP_BugTracker.Models;
using TOTP_BugTracker.Models.ViewModels;
using TOTP_BugTracker.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TOTP_BugTracker.Controllers
{

    public class UserRolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly ICompanyService _companyService;
        private readonly IProjectService _projectService;

        public UserRolesController(IRolesService rolesService,
                                   ICompanyService companyService,
                                   IProjectService projectService)
        {
            _rolesService = rolesService;
            _companyService = companyService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            // Add an instance of the ViewModel as a List
            List<ManageUserRolesViewModel> model = new();


            // Get CompanyId
            int companyId = User.Identity!.GetCompanyId();


            // Get all company users
            List<BTUser> members = await _companyService.GetMembersAsync(companyId);

            // loop over the users to populate the ViewModel

            // - Instantiate ViewModel
            // - use _rolesService
            // - Create multi-selects
            foreach (BTUser user in members)
            {
                ManageUserRolesViewModel viewModel = new();

                viewModel.BTUser = user;

                IEnumerable<string> selected = await _rolesService.GetUserRolesAsync(user);

                viewModel.Roles = new MultiSelectList(await _rolesService.GetRolesAsync(), "Name", "Name", selected);

                model.Add(viewModel);

            }


            // Return the model to the View




            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {
            // Get the company Id

            int companyId = User.Identity!.GetCompanyId();

            // Instantiate the BTUser
            BTUser btUser = (await _companyService.GetMembersAsync(companyId)).FirstOrDefault(u => u.Id == member.BTUser.Id);

            // Get Roles for the User
            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(btUser);

            // Grab the selected role
            string? userRole = member.SelectedRoles!.FirstOrDefault()!;

            if (!string.IsNullOrEmpty(userRole))
            {
                // Remove User from their roles
                if (await _rolesService.RemoveUserFromRolesAsync(btUser, roles))
                {
                    // Add user to the new role
                    await _rolesService.AddUserToRoleAsync(btUser, userRole);

                }

            }


            // Navigate back to the View
            return RedirectToAction(nameof(ManageUserRoles));

        }


    }
}
