using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Models.ProductsDto;

public record ProductDto(
    int Id, 
    string Name,
    string Description,
    decimal Price,
    decimal Discount,
    int Stock,
    Category Category,
    string PhotoFilename);