namespace TransportationAttendance.Domain.Entities.Central;

public class RegtUser
{
    public int UserId { get; init; }
    public string? Username { get; init; }
    public string? UserPassword { get; init; }
    public string? RoleIdCommaSep { get; init; }
    public string? FullNameOfficialAr { get; init; }
    public string? FullNameOfficialEn { get; init; }
    public string? EmailId { get; init; }
    public string? MobileNo { get; init; }
    public string? NationalId { get; init; }
    public bool? IsActive { get; init; }
    public string? UserLanguage { get; init; }
}
