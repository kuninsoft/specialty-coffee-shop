using Microsoft.EntityFrameworkCore.Design;

namespace SpecialtyCoffeeShop.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}