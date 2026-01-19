namespace SpecialtyCoffeeShop.Models.Dto;

public record CatalogDto(
    ICollection<ProductSummaryDto> Products);