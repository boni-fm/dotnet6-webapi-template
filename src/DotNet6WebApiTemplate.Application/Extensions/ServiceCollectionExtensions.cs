using DotNet6WebApiTemplate.Application.Interfaces;
using DotNet6WebApiTemplate.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet6WebApiTemplate.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Add Application Services
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}