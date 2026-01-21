using System.ComponentModel.DataAnnotations;

namespace SpecialtyCoffeeShop.Models.RequestDto;

public class AuthDto
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
}