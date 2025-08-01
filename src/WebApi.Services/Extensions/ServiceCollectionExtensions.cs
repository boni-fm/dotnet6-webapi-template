using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Services.Data;
using WebApi.Services.Repositories;
using WebApi.Services.Services;

namespace WebApi.Services.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add all application services including database, repositories, and business services
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext with PostgreSQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Add AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Add Repositories
        services.AddScoped<IProductRepository, ProductRepository>();

        // Add Services
        services.AddScoped<IProductService, ProductService>();

        // Add Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }
}