using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Models.Dto;

public record ProductSummaryDto(
    int Id, 
    string Name,
    decimal Price,
    decimal Discount,
    Category Category,
    int Stock,
    string PhotoFilename);