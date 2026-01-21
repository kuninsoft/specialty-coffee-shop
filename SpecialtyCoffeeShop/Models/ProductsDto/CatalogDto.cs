namespace SpecialtyCoffeeShop.Models.ProductsDto;

public record CatalogDto(
    ICollection<ProductSummaryDto> Products);