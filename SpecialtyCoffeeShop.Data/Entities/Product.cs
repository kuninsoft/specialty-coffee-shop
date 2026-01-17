using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecialtyCoffeeShop.Data.Entities;

public class Product
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

    [Required, MaxLength(100)] public string Name { get; set; }
    
    [Required, MaxLength(3000)] public string Description { get; set; }
    
    [Required] public decimal Price { get; set; }
    
    public decimal CurrentDiscount { get; set; }

    [Required] public Category Category { get; set; }
    
    [MaxLength(500)] public string PhotoFilename { get; set; }
}