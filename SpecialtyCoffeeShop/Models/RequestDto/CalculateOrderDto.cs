using System.ComponentModel.DataAnnotations;

namespace SpecialtyCoffeeShop.Models.RequestDto;

public class CalculateOrderDto
{
    [Required]
    public ICollection<OrderItemDto> Items { get; set; }
}