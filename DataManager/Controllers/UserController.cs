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

        // GET: api/User/5
        [HttpGet]
        [ResponseType(typeof(UserModel))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                UserData data = new UserData();
                string id = RequestContext.Principal.Identity.GetUserId();
                var user = await data.GetUser(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // POST: api/User
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
