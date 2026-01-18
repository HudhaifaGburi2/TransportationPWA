using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportationAttendance.Domain.Entities;

/// <summary>
/// Bus entity for transportation management.
/// Matches actual database schema from 001_CreatePhase1Schema_v2.sql
/// </summary>
[Table("Buses")]
public class Bus
{
    [Key]
    [Column("BusId")]
    public Guid Id { get; private set; }
    
    [Column("BusNumber")]
    public string BusNumber { get; private set; } = string.Empty;
    
    [Column("PeriodId")]
    public int PeriodId { get; private set; }
    
    [Column("RouteId")]
    public Guid? RouteId { get; private set; }
    
    [Column("DriverName")]
    public string? DriverName { get; private set; }
    
    [Column("DriverPhoneNumber")]
    public string? DriverPhoneNumber { get; private set; }
    
    [Column("Capacity")]
    public int Capacity { get; private set; }
    
    [Column("IsActive")]
    public bool IsActive { get; private set; }
    
    [Column("IsMerged")]
    public bool IsMerged { get; private set; }
    
    [Column("MergedWithBusId")]
    public Guid? MergedWithBusId { get; private set; }
    
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

    private Bus() 
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public static Bus Create(
        string busNumber,
        int periodId,
        int capacity = 30,
        string? driverName = null,
        string? driverPhoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(busNumber))
            throw new ArgumentException("Bus number is required.", nameof(busNumber));

        return new Bus
        {
            BusNumber = busNumber.Trim(),
            PeriodId = periodId,
            Capacity = capacity,
            DriverName = driverName,
            DriverPhoneNumber = driverPhoneNumber,
            IsActive = true
        };
    }

    public void Update(
        string busNumber,
        int periodId,
        int capacity,
        string? driverName,
        string? driverPhoneNumber)
    {
        BusNumber = busNumber.Trim();
        PeriodId = periodId;
        Capacity = capacity;
        DriverName = driverName;
        DriverPhoneNumber = driverPhoneNumber;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignRoute(Guid? routeId)
    {
        RouteId = routeId;
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

    public void MarkAsMerged(Guid mergedWithBusId)
    {
        IsMerged = true;
        MergedWithBusId = mergedWithBusId;
        UpdatedAt = DateTime.UtcNow;
    }
}
