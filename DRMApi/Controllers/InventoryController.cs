using DRMDataManagerLibrary.Data;
using DRMDataManagerLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DRMApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IConfiguration _config;

    public InventoryController(IConfiguration config)
    {
        _config = config;
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<IResult> Get()
    {
        try
        {
            InventoryData data = new(_config);

            var item = await data.GetAll();
            return Results.Ok(item);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IResult> Post(InventoryItemModel item)
    {
        try
        {
            InventoryData data = new(_config);

            await data.Add(item);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
