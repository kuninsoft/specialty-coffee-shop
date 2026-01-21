using System.ComponentModel.DataAnnotations;

namespace SpecialtyCoffeeShop.Models.CheckoutDto;

public class CalculateOrderDto
{
    [Required]
    public ICollection<OrderItemDto> Items { get; set; }
}