using DRMDataManagerLibrary.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DRMApi.Controllers;

[Authorize(Roles = "Cashier")]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IConfiguration _config;

    public ProductController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            ProductData data = new(_config);

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
