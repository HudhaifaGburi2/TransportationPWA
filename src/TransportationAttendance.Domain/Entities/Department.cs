namespace TransportationAttendance.Domain.Entities;

/// <summary>
/// Represents a department (e.g., MEN, WOMEN) that organizes buses and drivers.
/// Supports future multi-department expansion.
/// </summary>
public class Department : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string NameAr { get; private set; } = string.Empty;
    public string NameEn { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    // Navigation properties
    public ICollection<Driver> Drivers { get; private set; } = new List<Driver>();
    public ICollection<Bus> Buses { get; private set; } = new List<Bus>();

    private Department() { }

    public static Department Create(
        string code,
        string nameAr,
        string nameEn)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Department code is required.", nameof(code));

        if (string.IsNullOrWhiteSpace(nameAr))
            throw new ArgumentException("Arabic name is required.", nameof(nameAr));

        return new Department
        {
            Code = code.ToUpperInvariant().Trim(),
            NameAr = nameAr.Trim(),
            NameEn = nameEn?.Trim() ?? string.Empty,
            IsActive = true
        };
    }

    public void Update(string nameAr, string nameEn)
    {
        if (string.IsNullOrWhiteSpace(nameAr))
            throw new ArgumentException("Arabic name is required.", nameof(nameAr));

        NameAr = nameAr.Trim();
        NameEn = nameEn?.Trim() ?? string.Empty;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
