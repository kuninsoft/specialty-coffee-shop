using SpecialtyCoffeeShop.Data.Entities;
using SpecialtyCoffeeShop.Data.UnitOfWork;
using SpecialtyCoffeeShop.Models.Dto;
using SpecialtyCoffeeShop.Models.RequestDto;

namespace SpecialtyCoffeeShop.Services;

public class ProductsService(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
    ILogger<ProductsService> logger) : IProductsService
{
    public async Task<CatalogDto> GetProductsByCategoryAsync(CategoryDto category)
    {
        List<Product> products;

        if (category is CategoryDto.All)
        {
            products = await unitOfWork.Products.GetAsync();
        }
        else
        {
            products = await unitOfWork.Products.GetAsync(p => p.Category == ToEntityCategory(category));
        }

        return new CatalogDto(
            products.Select(product =>
                new ProductSummaryDto(
                    product.Id,
                    product.Name,
                    product.Price,
                    product.CurrentDiscount,
                    product.Category,
                    product.PhotoFilename
                )).ToList());
    }

    public async Task<ProductDto> GetProductOrDefaultAsync(int id)
    {
        Product product = await unitOfWork.Products.GetByIdAsync(id);

        if (product is null)
        {
            return null;
        }

        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.CurrentDiscount,
            product.Stock,
            product.Category,
            product.PhotoFilename);
    }

    public async Task AddProductAsync(AddProductDto body, IFormFile image)
    {
        string filename = await SaveImage(image);

        var product = new Product
        {
            Name = body.Name,
            Description = body.Description,
            Price = body.Price,
            CurrentDiscount = body.Discount,
            Category = body.Category,
            Stock = body.Stock,
            PhotoFilename = filename
        };

        try
        {
            unitOfWork.Products.Add(product);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            DeleteImage(filename);
            
            throw;
        }
    }

    public async Task UpdateProductAsync(int id, UpdateProductDto body)
    {
        Product product = await unitOfWork.Products.GetByIdAsync(id);

        if (product is null)
        {
            throw new KeyNotFoundException();
        }
        
        if (!string.IsNullOrWhiteSpace(body.Name))
        {
            product.Name = body.Name;
        }

        if (!string.IsNullOrWhiteSpace(body.Description))
        {
            product.Description = body.Description;
        }

        if (body.Stock.HasValue)
        {
            product.Stock = body.Stock.Value;
        }

        if (body.Category.HasValue)
        {
            product.Category = body.Category.Value;
        }
        
        // Price and Discount need some additional checks to ensure consistency
        if (body is { Price: not null, Discount: not null })
        {
            product.Price = body.Price.Value;
            product.CurrentDiscount = body.Discount.Value;
        } 
        else if (body.Price.HasValue)
        {
            if (body.Price < product.CurrentDiscount)
            {
                throw new InvalidOperationException("New price cannot be lower than Discount");
            }
            
            product.Price = body.Price.Value;
        }
        else if (body.Discount.HasValue)
        {
            if (product.Price < body.Discount)
            {
                throw new InvalidOperationException("Discount cannot be higher than Price");
            }
            
            product.CurrentDiscount = body.Discount.Value;
        }

        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        Product product = await unitOfWork.Products.GetByIdAsync(id);

        if (product is null)
        {
            throw new KeyNotFoundException();
        }
        
        bool deletionResult = await unitOfWork.Products.DeleteById(id);

        if (deletionResult)
        {
            DeleteImage(product.PhotoFilename);
        }
        
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<string> SaveImage(IFormFile image)
    {
        if (image is null || image.Length == 0)
        {
            throw new InvalidOperationException("Image is required");
        }
        
        string uploadPath = Path.Combine(hostEnvironment.WebRootPath, "product-photos");

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }
        
        string filetype = Path.GetExtension(image.FileName);
        string filename = Guid.NewGuid() + filetype;

        string filePath = Path.Combine(uploadPath, filename);

        await using Stream fileStream = new FileStream(filePath, FileMode.CreateNew);
        await image.CopyToAsync(fileStream);

        return filename;
    }

    private void DeleteImage(string filename)
    {
        logger.LogDebug("Delete image requested");
        
        string uploadPath = Path.Combine(hostEnvironment.WebRootPath, "product-photos");
        
        if (!Directory.Exists(uploadPath))
        {
            return;
        }
        
        string filePath = Path.Combine(uploadPath, filename);

        try
        {
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to delete product photo");
        }
    }

    private static Category ToEntityCategory(CategoryDto category)
    {
        return category switch
        {
            CategoryDto.Beans => Category.Beans,
            CategoryDto.Equipment => Category.Equipment,
            CategoryDto.Accessories => Category.Accessories,
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, null)
        };
    }
}