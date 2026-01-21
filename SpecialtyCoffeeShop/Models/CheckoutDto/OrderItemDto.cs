using System.ComponentModel.DataAnnotations;

namespace SpecialtyCoffeeShop.Models.CheckoutDto;

public class OrderItemDto
{
    [Required, Range(1, int.MaxValue)] public int Id { get; set; }
    [Required, Range(1, int.MaxValue)] public int Quantity { get; set; }
}