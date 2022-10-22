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
    [Route("api/Sale/Report")]
    public async Task<IResult> GetReport()
    {
        try
        {
            SaleData data = new(_config);

            var report = await data.GetSaleReport();
            return Results.Ok(report);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Cashier")]
    [HttpPost]
    public async Task<IResult> Post(SaleModel sale)
    {
        try
        {
            SaleData data = new(_config);
            string cashierId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await data.Add(sale, cashierId);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
