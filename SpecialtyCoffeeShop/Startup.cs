using Microsoft.EntityFrameworkCore;
using SpecialtyCoffeeShop.Data;
using SpecialtyCoffeeShop.Data.Repositories;
using SpecialtyCoffeeShop.Data.UnitOfWork;
using SpecialtyCoffeeShop.Payment;
using SpecialtyCoffeeShop.Services;

namespace SpecialtyCoffeeShop;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; set; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddLogging();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(
                Configuration.GetConnectionString("CoffeeDatabase")
                ?? throw new InvalidOperationException("Please add CoffeeDatabase connection string to appsettings.json"))
            );
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IPaymentProvider, MockPaymentProvider>();

        services.AddTransient<IProductsService, ProductsService>();
        services.AddTransient<ICheckoutService, CheckoutService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}