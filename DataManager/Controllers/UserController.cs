using DataManager.Models;
using DRMDataManagerLibrary.Data;
using DRMDataManagerLibrary.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace DataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(UserModel))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                UserData data = new UserData();
                string id = RequestContext.Principal.Identity.GetUserId();
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
        [Route("api/User/Admin/GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                List<ApplicationUserModel> people = new List<ApplicationUserModel>();

                using (var context = new ApplicationDbContext())
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var users = await userManager.Users.ToListAsync();
                    var roles = await context.Roles.ToListAsync();

                    foreach (var user in users)
                    {
                        ApplicationUserModel _user = new ApplicationUserModel()
                        {
                            Id = user.Id,
                            EmailAddress = user.Email
                        };

                        foreach (var role in user.Roles)
                        {
                            _user.Roles.Add(new ApplicationUserRoleModel { Id = role.RoleId, Name = roles.Where(x => x.Id == role.RoleId).First().Name });
                        }

                        people.Add(_user);
                    }

                    return Ok(people);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/User/Admin/GetAllRoles")]
        public async Task<IHttpActionResult> GetAllRoles()
        {
            try
            {
                List<ApplicationUserRoleModel> roles = new List<ApplicationUserRoleModel>();

                using (var context = new ApplicationDbContext())
                {
                    var results = await context.Roles.ToListAsync();

                    foreach (var role in results)
                    {
                        roles.Add(new ApplicationUserRoleModel { Id = role.Id, Name = role.Name });
                    }

                    return Ok(roles);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/AddRole")]
        public async Task<IHttpActionResult> AddRole(UserRolePairModel user)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    await userManager.AddToRoleAsync(user.UserId, user.RoleName);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/User/Admin/RemoveRole")]
        public async Task<IHttpActionResult> RemoveRole(UserRolePairModel user)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    await userManager.RemoveFromRoleAsync(user.UserId, user.RoleName);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
