using Microsoft.Extensions.Caching.Memory;
using SpecialtyCoffeeShop.Models.ProductsDto;

namespace SpecialtyCoffeeShop.Caching;

public class CatalogCache(ILogger<CatalogCache> logger, IMemoryCache cache) : ICatalogCache
{
    private const string CatalogCacheKeyFormat = "Catalog:{0}";
    private const string ProductCacheKeyFormat = "Product:{0}";
    
    public CatalogDto GetCatalogOrDefault(CategoryDto category)
    {
        logger.LogTrace("Get catalog cache for {category}", category);
        
        return cache.Get<CatalogDto>(string.Format(CatalogCacheKeyFormat, (int) category));
    }

    public void SetCatalog(CatalogDto catalogDto, CategoryDto category)
    {
        logger.LogTrace("Request Set catalog cache for {category}", category);
        
        cache.Set(string.Format(CatalogCacheKeyFormat, (int) category), catalogDto);
        
        logger.LogTrace("Complete Set catalog cache for {category}", category);
    }

    public void InvalidateCatalog()
    {
        logger.LogTrace("Request Invalidate catalog cache");
        
        foreach (CategoryDto value in Enum.GetValues<CategoryDto>())
        {
            cache.Remove(string.Format(CatalogCacheKeyFormat, (int) value));
        }
        
        logger.LogTrace("Complete Invalidate catalog cache");
    }

    public ProductDto GetProductOrDefault(int id)
    {
        logger.LogTrace("Get product cache for id {id}", id);
        
        return cache.Get<ProductDto>(string.Format(ProductCacheKeyFormat, id));
    }

    public void SetProduct(int id, ProductDto productDto)
    {
        logger.LogTrace("Request Set product cache for id {id}", id);
        
        cache.Set(string.Format(ProductCacheKeyFormat, id), productDto);
        
        logger.LogTrace("Complete Set product cache for id {id}", id);
    }

    public void InvalidateProduct(int id)
    {
        logger.LogTrace("Request Invalidate product cache for id {id}", id);
        
        cache.Remove(string.Format(ProductCacheKeyFormat, id));
        
        logger.LogTrace("Complete Invalidate product cache for id {id}", id);
    }
}