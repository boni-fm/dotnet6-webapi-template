namespace WebApi.Base.Interfaces;

/// <summary>
/// Base interface for all entities
/// </summary>
public interface IEntity
{
    int Id { get; set; }
}

/// <summary>
/// Interface for entities with audit fields
/// </summary>
public interface IAuditableEntity : IEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Generic repository interface
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : class, IEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

/// <summary>
/// Unit of work pattern interface
/// </summary>
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}