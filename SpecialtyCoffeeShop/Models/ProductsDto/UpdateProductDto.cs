using System.ComponentModel.DataAnnotations;
using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Models.ProductsDto;

public class UpdateProductDto
{
    [MaxLength(100)]
    public string Name { get; set; }
    
    [MaxLength(3000)] 
    public string Description { get; set; }
    
    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? Discount { get; set; }
    
    [Range(0, int.MaxValue)] 
    public int? Stock { get; set; }
    
    public Category? Category { get; set; }
}