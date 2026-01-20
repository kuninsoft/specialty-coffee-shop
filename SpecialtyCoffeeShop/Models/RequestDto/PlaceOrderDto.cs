using System.ComponentModel.DataAnnotations;

namespace SpecialtyCoffeeShop.Models.RequestDto;

public class PlaceOrderDto
{
    [Required]
    public ICollection<OrderItemDto> Items { get; set; }

    [Required]
    public ShippingInfoDto ShippingInfo { get; set; }
}