using SpecialtyCoffeeShop.Data.Repositories;

namespace SpecialtyCoffeeShop.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }

    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

    Task<int> SaveChangesAsync();
}