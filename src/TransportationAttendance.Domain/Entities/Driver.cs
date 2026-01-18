namespace TransportationAttendance.Domain.Entities;

/// <summary>
/// Represents a bus driver as a first-class entity.
/// A driver can be assigned to one bus per shift/period.
/// </summary>
public class Driver : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public Guid DepartmentId { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public Department Department { get; private set; } = null!;
    public ICollection<Bus> Buses { get; private set; } = new List<Bus>();

    private Driver() { }

    public static Driver Create(
        string fullName,
        string phoneNumber,
        Guid departmentId)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Driver name is required.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required.", nameof(phoneNumber));

        return new Driver
        {
            FullName = fullName.Trim(),
            PhoneNumber = NormalizePhoneNumber(phoneNumber),
            DepartmentId = departmentId,
            IsActive = true
        };
    }

    public void Update(string fullName, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Driver name is required.", nameof(fullName));

        FullName = fullName.Trim();
        PhoneNumber = NormalizePhoneNumber(phoneNumber);
    }

    public void UpdateDepartment(Guid departmentId)
    {
        DepartmentId = departmentId;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    /// <summary>
    /// Normalizes phone number by removing spaces and ensuring consistent format.
    /// Stores without country code - country code can be added at presentation layer if needed.
    /// </summary>
    private static string NormalizePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        // Remove all non-digit characters
        var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

        // If starts with country code (966), remove it
        if (digits.StartsWith("966") && digits.Length > 9)
            digits = digits.Substring(3);

        // If starts with 0, keep it (local format)
        // Otherwise return as-is
        return digits;
    }
}
