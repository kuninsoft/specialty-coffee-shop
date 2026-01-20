using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SpecialtyCoffeeShop.Data;
using SpecialtyCoffeeShop.Data.Repositories;
using SpecialtyCoffeeShop.Data.UnitOfWork;
using SpecialtyCoffeeShop.Models;
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

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "SpecialtyCoffeeShop.Auth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.Configure<AdminCredentials>(Configuration.GetSection("AdminCredentials"));

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