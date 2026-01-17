using Microsoft.EntityFrameworkCore.Storage;
using SpecialtyCoffeeShop.Data.Repositories;

namespace SpecialtyCoffeeShop.Data.UnitOfWork;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction _transaction;
    
    public IProductRepository Products { get; } = new ProductRepository(dbContext);
    public IOrderRepository Orders { get; } = new OrderRepository(dbContext);
    
    public async Task BeginTransactionAsync()
    {
        _transaction = await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
            }
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await DisposeTransactionAsync();
        }
    }

    public Task<int> SaveChangesAsync() => dbContext.SaveChangesAsync();
    
    private async Task DisposeTransactionAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
        await DisposeTransactionAsync();
    }
}