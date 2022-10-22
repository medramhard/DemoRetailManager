using DataManager.Models;
using DRMDataManagerLibrary.Data;
using DRMDataManagerLibrary.Models;
using Microsoft.AspNet.Identity;
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
    public class SaleController : ApiController
    {
        private readonly IConfiguration _config;

        public SaleController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        [Route("api/Report")]
        public async Task<IHttpActionResult> GetReport()
        {
            try
            {
                SaleData data = new SaleData(_config);

                var report = await data.GetSaleReport();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(SaleModel sale)
        {
            try
            {
                SaleData data = new SaleData(_config);
                string cashierId = RequestContext.Principal.Identity.GetUserId();

                await data.Add(sale, cashierId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
