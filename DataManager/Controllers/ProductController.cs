using DRMDataManagerLibrary.Data;
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
    [Authorize(Roles = "Cashier")]
    public class ProductController : ApiController
    {
        private readonly IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IHttpActionResult> Get()
        {
            try
            {
                ProductData data = new ProductData(_config);

                var products = await data.GetAll();

                if (products == null)
                {
                    return NotFound();
                }

                return Ok(products);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
