using DRMDataManagerLibrary.Data;
using DRMDataManagerLibrary.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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
    }
}
