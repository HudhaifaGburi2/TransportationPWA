namespace TransportationAttendance.Domain.Entities;

/// <summary>
/// Bus entity for transportation management.
/// </summary>
public class Bus
{
    public Guid Id { get; private set; }
    public string BusNumber { get; private set; } = string.Empty;
    public string LicensePlate { get; private set; } = string.Empty;
    public int Capacity { get; private set; }
    public string? Model { get; private set; }
    public int? Year { get; private set; }
    public int Status { get; private set; }
    public DateTime? LastMaintenanceDate { get; private set; }
    public DateTime? NextMaintenanceDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public Guid? UpdatedBy { get; private set; }

    private Bus() 
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public static Bus Create(
        string busNumber,
        string licensePlate,
        int capacity = 30,
        string? model = null,
        int? year = null)
    {
        if (string.IsNullOrWhiteSpace(busNumber))
            throw new ArgumentException("Bus number is required.", nameof(busNumber));

        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new ArgumentException("License plate is required.", nameof(licensePlate));

        return new Bus
        {
            BusNumber = busNumber.Trim(),
            LicensePlate = licensePlate.Trim(),
            Capacity = capacity,
            Model = model,
            Year = year,
            Status = 1 // Active
        };
    }

    public void Update(
        string busNumber,
        string licensePlate,
        int capacity,
        string? model,
        int? year)
    {
        BusNumber = busNumber.Trim();
        LicensePlate = licensePlate.Trim();
        Capacity = capacity;
        Model = model;
        Year = year;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetMaintenanceSchedule(DateTime? lastMaintenance, DateTime? nextMaintenance)
    {
        LastMaintenanceDate = lastMaintenance;
        NextMaintenanceDate = nextMaintenance;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() 
    { 
        Status = 1; 
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate() 
    { 
        Status = 0; 
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsActive => Status == 1;
}
