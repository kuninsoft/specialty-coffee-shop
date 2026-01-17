using System.Linq.Expressions;
using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Data.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAsync(Expression<Func<Product, bool>> filter = null);
    Task<Product> GetByIdAsync(int id);
    void Update(Product product);
    void Add(Product product);
    Task<bool> DeleteById(int id);
}