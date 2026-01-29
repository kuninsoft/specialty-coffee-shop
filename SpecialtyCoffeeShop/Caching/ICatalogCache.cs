using SpecialtyCoffeeShop.Models.ProductsDto;

namespace SpecialtyCoffeeShop.Caching;

public interface ICatalogCache
{
    CatalogDto GetCatalogOrDefault(CategoryDto category);
    void SetCatalog(CatalogDto catalogDto, CategoryDto category);
    void InvalidateCatalog();

    ProductDto GetProductOrDefault(int id);
    void SetProduct(int id, ProductDto productDto);
    void InvalidateProduct(int id);
}