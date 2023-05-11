using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimiiformesWebApplication.Data;
using SimiiformesWebApplication.Models;
using SimiiformesWebApplication.ViewModels;

namespace SimiiformesWebApplication.Controllers
{
    [Authorize(Roles = nameof(Role.SystemAdmin))]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        // GET: UserViewModels
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Select(c => new UserViewModel
            {
                Id = c.Id,
                UserName = c.UserName,
                //Role = string.Join(", ", _userManager.GetRolesAsync(c).Result.ToArray())
            }).ToListAsync();

            foreach(var user in users)
            {
                foreach( var roleId in _context.UserRoles.Where(c => c.UserId == user.Id).Select(c => c.RoleId).ToList())
                {
                    user.Role += _context.Roles.Where(c => c.Id == roleId).Select(c => c.Name).FirstOrDefault() + ", ";
                }
                user.Role = user.Role.Remove(user.Role.Length - 2);
            }

            return View(users);
        }

        // GET: UserViewModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null) return NotFound();

            var userRoleViewModel = new UserRoleViewModel { Id = user.Id, UserName = user.UserName };

            foreach (var role in _userManager.GetRolesAsync(user).Result)
            {
                if(role == Role.Manager.ToString())
                {
                    userRoleViewModel.ManagerIsChecked = true;
                }

                if (role == Role.Administrator.ToString())
                {
                    userRoleViewModel.AdministratorIsChecked = true;
                }
                if(role == Role.SystemAdmin.ToString())
                {
                    userRoleViewModel.SystemAdminIsChecked = true;
                }
            }

            return View(userRoleViewModel);
        }

        // POST: UserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ManagerIsChecked,AdministratorIsChecked,SystemAdminIsChecked")] UserRoleViewModel userRoleViewModel)
        {
            if (id != userRoleViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(userRoleViewModel.Id);
                    if (user == null) return NotFound();

                    await _userManager.RemoveFromRolesAsync( user , _userManager.GetRolesAsync(user).Result.ToArray());

                    if (userRoleViewModel.ManagerIsChecked)
                    {
                        await _userManager.AddToRoleAsync(user, Role.Manager.ToString());
                    }

                    if (userRoleViewModel.AdministratorIsChecked)
                    {
                        await _userManager.AddToRoleAsync(user, Role.Administrator.ToString());
                    }

                    if (userRoleViewModel.SystemAdminIsChecked)
                    {
                        await _userManager.AddToRoleAsync(user, Role.SystemAdmin.ToString());
                    }                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserViewModelExists(userRoleViewModel.Id))
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
            return View(userRoleViewModel);
        }

        private bool UserViewModelExists(string id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
