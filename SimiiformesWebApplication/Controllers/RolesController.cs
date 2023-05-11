using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimiiformesWebApplication.Models;
using System.Data;

namespace SimiiformesWebApplication.Controllers
{
    public class RolesController : Controller
    {
        //https://learn.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-7.0

        //adminisztrátor
        [Authorize(Roles = "Administrator")]
        public class AdministratorController : Controller
        {
            public IActionResult Index() =>
                Content("Administrator");
        }
        
        //vezető
        [Authorize(Roles = "Manager")]
        public class ManagerController : Controller
        {
            public IActionResult Index() =>
                Content("Manager");
        }
        
        //rendszergazda
        [Authorize(Roles = "SystemAdmin")]
        public class SystemAdminController : Controller
        {
            public IActionResult Index() =>
                Content("SystemAdmin");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
