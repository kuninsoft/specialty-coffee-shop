using Microsoft.AspNetCore.Mvc;
using SpecialtyCoffeeShop.Caching;
using SpecialtyCoffeeShop.Models.CheckoutDto;
using SpecialtyCoffeeShop.Services;

namespace SpecialtyCoffeeShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckoutController(ICheckoutService checkoutService, ICatalogCache catalogCache,
    ILogger<CheckoutController> logger) : ControllerBase
{
    // POST api/<CheckoutController>/CalculateOrderDetails
    [HttpPost(nameof(CalculateOrderDetails))]
    public async Task<ActionResult<OrderDetailsDto>> CalculateOrderDetails([FromBody] CalculateOrderDto order)
    {
        if (order.Items is not { Count: > 0 })
        {
            ModelState.AddModelError(nameof(order.Items), "Items collection must have a value");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            OrderDetailsDto details = await checkoutService.CalculateOrderDetailsAsync(order);

            logger.LogTrace("Calculated order details");

            return details;
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogDebug(ex, "Unknown product id passed");
                
            return NotFound("Unknown product ID passed");
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Not enough stock");

            return BadRequest("Not enough stock");
        }
    }

    // POST api/<CheckoutController>/PlaceOrder
    [HttpPost(nameof(PlaceOrder))]
    public async Task<ActionResult<OrderInfoDto>> PlaceOrder([FromBody] PlaceOrderDto order)
    {
        if (order.Items is not { Count: > 0 })
        {
            ModelState.AddModelError(nameof(order.Items), "Items collection must have a value");
        }
            
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            logger.LogInformation("Placing order");
            OrderInfoDto orderInfo = await checkoutService.PlaceOrderAsync(order);

            logger.LogInformation("Order {id} success", orderInfo.Id);
            
            InvalidateAffectedProductsCache(order.Items);
            
            return orderInfo;
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogDebug(ex, "Unknown product id passed");
                
            return NotFound("Unknown product ID passed");
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Not enough stock");

            return BadRequest("Not enough stock");
        }
    }

    private void InvalidateAffectedProductsCache(IEnumerable<OrderItemDto> orderItems)
    {
        foreach (OrderItemDto item in orderItems)
        {
            catalogCache.InvalidateProduct(item.Id);
        }
        
        catalogCache.InvalidateCatalog();
    }
}