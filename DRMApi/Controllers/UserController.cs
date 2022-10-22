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

    public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            UserData data = new();
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await data.GetUser(id);

            return Results.Ok(user);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("api/User/Admin/GetAll")]
    public async Task<IResult> GetAll()
    {
        try
        {
            List<ApplicationUserModel> people = new List<ApplicationUserModel>();

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

            return Results.Ok(people);

        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("api/User/Admin/GetAllRoles")]
    public async Task<IResult> GetAllRoles()
    {
        try
        {
            List<ApplicationUserRoleModel> roles = new();

            var results = await _context.Roles.ToListAsync();

            foreach (var role in results)
            {
                roles.Add(new ApplicationUserRoleModel { Id = role.Id, Name = role.Name });
            }

            return Results.Ok(roles);

        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("api/User/Admin/AddRole")]
    public async Task<IResult> AddRole(UserRolePairModel pairing)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.AddToRoleAsync(user,pairing.RoleName);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [Route("api/User/Admin/RemoveRole")]
    public async Task<IResult> RemoveRole(UserRolePairModel pairing)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);
            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
