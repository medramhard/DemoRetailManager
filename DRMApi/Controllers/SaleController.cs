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
    private readonly ISaleData _data;

    public SaleController(ISaleData data)
    {
        _data = data;
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet]
    [Route("Report")]
    public async Task<IActionResult> GetReport()
    {
        try
        {
            return Ok(await _data.GetSaleReport());
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
            string cashierId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _data.Add(sale, cashierId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
