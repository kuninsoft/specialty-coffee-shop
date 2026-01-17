using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Data.Repositories;

public interface IOrderRepository
{
    public void Add(Order order);
}