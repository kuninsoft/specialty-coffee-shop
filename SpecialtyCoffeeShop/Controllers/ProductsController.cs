using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpecialtyCoffeeShop.Models.ProductsDto;
using SpecialtyCoffeeShop.Models.RequestDto;
using SpecialtyCoffeeShop.Services;

namespace SpecialtyCoffeeShop.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductsService productsService, ILogger<ProductsController> logger) : ControllerBase
{
    // GET api/<ProductsController>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<CatalogDto>> Get([FromQuery] CategoryDto category = CategoryDto.All)
    {
        try
        {
            return await productsService.GetProductsByCategoryAsync(category);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get products by category");

            return BadRequest();
        }
    }

    // GET api/<ProductsController>/5
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> Get(int id)
    {
        ProductDto product = await productsService.GetProductOrDefaultAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return product;
    }

    // POST api/<ProductsController>
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] AddProductDto body, IFormFile image)
    {
        if (body.Price < body.Discount)
        {
            ModelState.AddModelError(nameof(body.Discount), "Discount can't be larger than Price");
        }
        
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }
        
        if (image is null || image.Length == 0)
        {
            logger.LogDebug("Tried uploading without product photo");
            
            return BadRequest("Image is required");
        }

        await productsService.AddProductAsync(body, image);

        return Created();
    }

    // PUT api/<ProductsController>/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateProductDto body)
    {
        if (body is { Price: not null, Discount: not null } 
            && body.Price < body.Discount)
        {
            ModelState.AddModelError(nameof(body.Discount), "Discount can't be larger than Price");
        }
        
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await productsService.UpdateProductAsync(id, body);
            
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogDebug(ex, "Unknown product ID passed");
            
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Failed to update product entity");

            return BadRequest();
        }
    }

    // DELETE api/<ProductsController>/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await productsService.DeleteProductAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return Ok();
    }
}