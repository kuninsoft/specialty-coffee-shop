using System.ComponentModel.DataAnnotations;
using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Models.RequestDto;

public class AddProductDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; }
    
    [Required, MaxLength(3000)] 
    public string Description { get; set; }
    
    [Required, Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, double.MaxValue)] 
    public decimal Discount { get; set; } = 0;
    
    [Required, Range(0, int.MaxValue)] 
    public int Stock { get; set; }
    
    [Required] 
    public Category Category { get; set; }
}