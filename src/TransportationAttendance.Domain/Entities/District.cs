namespace TransportationAttendance.Domain.Entities;

public class District : BaseEntity
{
    public string DistrictNameAr { get; private set; } = string.Empty;
    public string? DistrictNameEn { get; private set; }
    public bool IsActive { get; private set; }

    private District() { }

    public static District Create(string districtNameAr, string? districtNameEn = null)
    {
        return new District
        {
            DistrictNameAr = districtNameAr,
            DistrictNameEn = districtNameEn,
            IsActive = true
        };
    }

    public void Update(string districtNameAr, string? districtNameEn)
    {
        DistrictNameAr = districtNameAr;
        DistrictNameEn = districtNameEn;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
