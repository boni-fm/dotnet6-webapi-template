using WebApi.Base.Interfaces;
using WebApi.Services.Data.Entities;

namespace WebApi.Services.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<IEnumerable<Product>> GetActiveProductsAsync();
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(Data.ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await Task.FromResult(_dbSet.Where(p => p.Category == category).ToList());
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await Task.FromResult(_dbSet.Where(p => p.IsActive).ToList());
    }
}