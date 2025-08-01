using WebApi.Base.Interfaces;

namespace WebApi.Services.Data.Entities;

/// <summary>
/// Base entity with audit fields
/// </summary>
public abstract class BaseEntity : IAuditableEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}