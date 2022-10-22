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
    public async Task<IResult> Get()
    {
        try
        {
            ProductData data = new();

            var products = await data.GetAll();

            if (products == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(products);
        }
        catch (Exception ex)
        {

            return Results.BadRequest(ex.Message);
        }
    }
}
