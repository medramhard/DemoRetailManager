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
    public async Task<IActionResult> Get()
    {
        try
        {
            InventoryData data = new(_config);

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
    public async Task<IActionResult> Post(InventoryItemModel item)
    {
        try
        {
            InventoryData data = new(_config);

            await data.Add(item);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
