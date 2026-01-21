namespace SpecialtyCoffeeShop.Models.CheckoutDto;

public class OrderDetailsDto
{
    public decimal SubtotalPrice { get; set; }
    public decimal Discount { get; set; } = 0;
    public decimal TotalPrice { get; set; } = 0;
}