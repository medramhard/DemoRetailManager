using DRMDataManagerLibrary.Data;
using DRMDataManagerLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        private readonly IConfiguration _config;

        public InventoryController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                InventoryData data = new InventoryData(_config);

                var item = await data.GetAll();
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(InventoryItemModel item)
        {
            try
            {
                InventoryData data = new InventoryData(_config);

                await data.Add(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
