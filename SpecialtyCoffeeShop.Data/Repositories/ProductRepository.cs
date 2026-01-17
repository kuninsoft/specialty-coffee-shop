using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Data.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public async Task<List<Product>> GetAsync(Expression<Func<Product, bool>> filter = null)
    {
        IQueryable<Product> query = dbContext.Products;

        if (filter is not null)
            query = query.Where(filter);

        return await query.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await dbContext.Products.FindAsync(id);
    }

    public void Update(Product product)
    {
        dbContext.Products.Update(product);
    }

    public void Add(Product product)
    {
        dbContext.Products.Add(product);
    }

    public async Task<bool> DeleteById(int id)
    {
        Product product = await dbContext.Products.FindAsync(id);

        if (product is null)
        {
            return false;
        }

        dbContext.Products.Remove(product);

        return true;
    }
}