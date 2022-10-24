using DRMDataManagerLibrary.Data;
using DRMDataManagerLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DRMApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SaleController : ControllerBase
{
    private readonly IConfiguration _config;

    public SaleController(IConfiguration config)
    {
        _config = config;
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet]
    [Route("Report")]
    public async Task<IActionResult> GetReport()
    {
        try
        {
            SaleData data = new(_config);

            var report = await data.GetSaleReport();
            return Ok(report);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    //[Authorize(Roles = "Cashier")]
    [HttpPost]
    public async Task<IActionResult> Post(SaleModel sale)
    {
        try
        {
            SaleData data = new(_config);
            string cashierId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await data.Add(sale, cashierId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
