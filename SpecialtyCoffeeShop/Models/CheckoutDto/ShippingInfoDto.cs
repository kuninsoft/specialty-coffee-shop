using System.ComponentModel.DataAnnotations;

namespace SpecialtyCoffeeShop.Models.CheckoutDto;

public class ShippingInfoDto
{
    [Required, MaxLength(300)] public string FullName { get; set; }

    [Required, MaxLength(50)] public string Country { get; set; }
    [Required, MaxLength(50)] public string Region { get; set; }
    [Required, MaxLength(50)] public string City { get; set; }
    [Required, MaxLength(400)] public string Address { get; set; }
 
    [MaxLength(15)] public string PhoneNumber { get; set; }
    [MaxLength(255)] public string Email { get; set; }
}