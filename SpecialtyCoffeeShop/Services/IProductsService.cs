using SpecialtyCoffeeShop.Models.Dto;
using SpecialtyCoffeeShop.Models.RequestDto;

namespace SpecialtyCoffeeShop.Services;

public interface IProductsService
{
    Task<CatalogDto> GetProductsByCategoryAsync(CategoryDto category);
    Task<ProductDto> GetProductOrDefaultAsync(int id);
    Task AddProductAsync(AddProductDto body, IFormFile image);
    Task UpdateProductAsync(int id, UpdateProductDto body);
    Task DeleteProductAsync(int id);
}