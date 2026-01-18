namespace TransportationAttendance.Domain.Entities;

/// <summary>
/// Driver entity for transportation management.
/// </summary>
public class Driver
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string LicenseNumber { get; private set; } = string.Empty;
    public DateTime LicenseExpiryDate { get; private set; }
    public string? EmployeeId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public Guid? UpdatedBy { get; private set; }

    private Driver() 
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public static Driver Create(
        string fullName,
        string phoneNumber,
        string licenseNumber,
        DateTime licenseExpiryDate,
        string? employeeId = null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Driver name is required.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required.", nameof(phoneNumber));

        if (string.IsNullOrWhiteSpace(licenseNumber))
            throw new ArgumentException("License number is required.", nameof(licenseNumber));

        return new Driver
        {
            FullName = fullName.Trim(),
            PhoneNumber = NormalizePhoneNumber(phoneNumber),
            LicenseNumber = licenseNumber.Trim(),
            LicenseExpiryDate = licenseExpiryDate,
            EmployeeId = employeeId,
            IsActive = true
        };
    }

    public void Update(
        string fullName,
        string phoneNumber,
        string licenseNumber,
        DateTime licenseExpiryDate,
        string? employeeId)
    {
        FullName = fullName.Trim();
        PhoneNumber = NormalizePhoneNumber(phoneNumber);
        LicenseNumber = licenseNumber.Trim();
        LicenseExpiryDate = licenseExpiryDate;
        EmployeeId = employeeId;
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

    private static string NormalizePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());
        if (digits.StartsWith("966") && digits.Length > 9)
            digits = digits.Substring(3);

        return digits;
    }
}
