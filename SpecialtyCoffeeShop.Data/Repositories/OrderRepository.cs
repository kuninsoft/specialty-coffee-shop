using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Data.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    public void Add(Order order)
    {
        dbContext.Add(order);
    }
}