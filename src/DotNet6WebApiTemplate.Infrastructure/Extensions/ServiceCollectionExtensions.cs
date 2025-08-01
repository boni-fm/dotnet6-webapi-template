using DotNet6WebApiTemplate.Domain.Interfaces;
using DotNet6WebApiTemplate.Infrastructure.Data;
using DotNet6WebApiTemplate.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotNet6WebApiTemplate.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Add Health Checks
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        // Add Repositories
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}