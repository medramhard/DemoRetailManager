using DRMApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DRMApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        private async Task<string[]> CreateRoles()
        {
            string[] roles = { "Admin", "Manager", "Cashier" };

            foreach (var role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);

                if (roleExist == false)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            return roles;
        }

        // The method below populates EF Database with Admin, and Manager users
        public async Task<IActionResult> Index()
        {
            var roles = await CreateRoles();

            var admin = new EFUserModel()
            {
                UserName = "Admin",
                Email = "admin@drm.com",
                EmailConfirmed = true
            };
            var user = admin.GetUser();
            var result = await _userManager.CreateAsync(user, "!A3fpnzUaeLs8");
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, roles);
            }

            var manager = new EFUserModel()
            {
                UserName = "Manager",
                Email = "manager@drm.com",
                EmailConfirmed = true
            };
            user = manager.GetUser();
            result = await _userManager.CreateAsync(user, "1apMn(XMkjvy)");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Manager");
            }

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