using DRMDataManagerLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DRMApi.Models;
using DRMApi.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DRMApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;

    public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration config)
    {
        _context = context;
        _userManager = userManager;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            UserData data = new(_config);
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await data.GetUser(id);

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("Admin/GetAll")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<ApplicationUserModel> people = new();

            var users = await _context.Users.ToListAsync();
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

            foreach (var user in users)
            {
                ApplicationUserModel _user = new ApplicationUserModel()
                {
                    Id = user.Id,
                    EmailAddress = user.Email
                };

                _user.Roles = userRoles.Where(x => x.UserId == _user.Id).Select(x => new ApplicationUserRoleModel() { Id = x.RoleId, Name = x.Name }).ToList();

                people.Add(_user);
            }

            return Ok(people);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("Admin/GetAllRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            List<ApplicationUserRoleModel> roles = new();

            var results = await _context.Roles.ToListAsync();

            foreach (var role in results)
            {
                roles.Add(new ApplicationUserRoleModel { Id = role.Id, Name = role.Name });
            }

            return Ok(roles);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("Admin/AddRole")]
    public async Task<IActionResult> AddRole(UserRolePairModel pairing)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.AddToRoleAsync(user,pairing.RoleName);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("Admin/RemoveRole")]
    public async Task<IActionResult> RemoveRole(UserRolePairModel pairing)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
