using System.ComponentModel.DataAnnotations;

namespace SpecialtyCoffeeShop.Models.CheckoutDto;

public class PlaceOrderDto
{
    [Required]
    public ICollection<OrderItemDto> Items { get; set; }

    [Required]
    public ShippingInfoDto ShippingInfo { get; set; }
}