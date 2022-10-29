using DRMDataManagerLibrary.Data.Interfaces;
using DRMDataManagerLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DRMApi.Controllers;

[Authorize(Roles = "Cashier")]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductData _data;

    public ProductController(IProductData data)
    {
        _data = data;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var products = await _data.GetAll();

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

    [HttpPut]
    public async Task<IActionResult> UpdateQuantity(ProductModel product)
    {
        try
        {
            await _data.Update(product);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
