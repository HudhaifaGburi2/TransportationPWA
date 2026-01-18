using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportationAttendance.Domain.Entities;

/// <summary>
/// Route entity for transportation management.
/// Matches actual database schema from 001_CreatePhase1Schema_v2.sql
/// </summary>
[Table("Routes")]
public class Route
{
    [Key]
    [Column("RouteId")]
    public Guid Id { get; private set; }
    
    [Column("RouteName")]
    public string Name { get; private set; } = string.Empty;
    
    [Column("RouteDescription")]
    public string? Description { get; private set; }
    
    [Column("IsActive")]
    public bool IsActive { get; private set; }
    
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; private set; }
    
    [Column("CreatedBy")]
    public Guid? CreatedBy { get; private set; }
    
    [Column("UpdatedAt")]
    public DateTime? UpdatedAt { get; private set; }
    
    [Column("UpdatedBy")]
    public Guid? UpdatedBy { get; private set; }
    
    [Column("IsDeleted")]
    public bool IsDeleted { get; private set; }
    
    [Column("DeletedAt")]
    public DateTime? DeletedAt { get; private set; }
    
    [Column("DeletedBy")]
    public Guid? DeletedBy { get; private set; }

    private Route() 
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public static Route Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Route name is required.", nameof(name));

        return new Route
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            IsActive = true
        };
    }

    public void Update(string name, string? description)
    {
        Name = name.Trim();
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() 
    { 
        IsActive = true; 
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate() 
    { 
        IsActive = false; 
        UpdatedAt = DateTime.UtcNow;
    }
}
