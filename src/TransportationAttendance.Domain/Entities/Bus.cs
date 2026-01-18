namespace TransportationAttendance.Domain.Entities;

public class Bus : BaseEntity
{
    // PlateNumber is the authoritative identifier from official CSV
    // Uniqueness enforced on (PlateNumber + PeriodId) - same plate can appear in different periods
    public string PlateNumber { get; private set; } = string.Empty;
    public int PeriodId { get; private set; }
    public Guid? RouteId { get; private set; }
    public Guid? DriverId { get; private set; }
    public Guid? DepartmentId { get; private set; }
    
    // Bus number for display (e.g., "1", "2", etc.)
    public string? BusNumber { get; private set; }
    
    // Legacy fields - kept for backward compatibility, will be phased out
    [Obsolete("Use Driver entity instead. This field is kept for legacy data migration.")]
    public string? DriverName { get; private set; }
    [Obsolete("Use Driver entity instead. This field is kept for legacy data migration.")]
    public string? DriverPhoneNumber { get; private set; }
    
    public int Capacity { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsMerged { get; private set; }
    public Guid? MergedWithBusId { get; private set; }

    // Navigation properties
    public Route? Route { get; private set; }
    public Driver? Driver { get; private set; }
    public Department? Department { get; private set; }
    public Bus? MergedWithBus { get; private set; }
    public ICollection<BusDistrict> BusDistricts { get; private set; } = new List<BusDistrict>();
    public ICollection<StudentBusAssignment> StudentAssignments { get; private set; } = new List<StudentBusAssignment>();

    private Bus() { }

    public static Bus Create(
        string plateNumber,
        int periodId,
        int capacity = 30,
        Guid? routeId = null,
        Guid? driverId = null,
        Guid? departmentId = null,
        string? busNumber = null)
    {
        if (string.IsNullOrWhiteSpace(plateNumber))
            throw new ArgumentException("Plate number is required.", nameof(plateNumber));

        return new Bus
        {
            PlateNumber = NormalizePlateNumber(plateNumber),
            PeriodId = periodId,
            Capacity = capacity,
            RouteId = routeId,
            DriverId = driverId,
            DepartmentId = departmentId,
            BusNumber = busNumber,
            IsActive = true,
            IsMerged = false
        };
    }

    public void Update(
        string plateNumber,
        int periodId,
        int capacity,
        Guid? routeId,
        Guid? driverId,
        Guid? departmentId,
        string? busNumber)
    {
        PlateNumber = NormalizePlateNumber(plateNumber);
        PeriodId = periodId;
        Capacity = capacity;
        RouteId = routeId;
        DriverId = driverId;
        DepartmentId = departmentId;
        BusNumber = busNumber;
    }

    public void AssignDriver(Guid driverId)
    {
        DriverId = driverId;
    }

    public void UnassignDriver()
    {
        DriverId = null;
    }

    public void AssignDepartment(Guid departmentId)
    {
        DepartmentId = departmentId;
    }

    /// <summary>
    /// Normalizes plate number by removing extra spaces and standardizing format.
    /// </summary>
    private static string NormalizePlateNumber(string plateNumber)
    {
        if (string.IsNullOrWhiteSpace(plateNumber))
            return string.Empty;

        // Normalize Arabic/English spaces and trim
        return plateNumber.Trim();
    }

    public void MergeWith(Guid targetBusId)
    {
        IsMerged = true;
        MergedWithBusId = targetBusId;
    }

    public void UnMerge()
    {
        IsMerged = false;
        MergedWithBusId = null;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
