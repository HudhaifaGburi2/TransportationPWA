namespace TransportationAttendance.Application.DTOs.NationalAddress;

/// <summary>
/// Response from National Address API lookup
/// </summary>
public record NationalAddressDto
{
    public bool Success { get; init; }
    public string ShortAddress { get; init; } = string.Empty;
    public string? FullAddress { get; init; }
    public string? City { get; init; }
    public string? District { get; init; }
    public string? Street { get; init; }
    public string? BuildingNumber { get; init; }
    public string? PostalCode { get; init; }
    public string? AdditionalNumber { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public string? ErrorMessage { get; init; }
}

/// <summary>
/// Response structure from Saudi National Address API
/// </summary>
public class NationalAddressApiResponse
{
    public List<AddressInfo>? Addresses { get; set; }
    public int? TotalPages { get; set; }
    public int? TotalRecords { get; set; }
}

public class AddressInfo
{
    public string? Title { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Street { get; set; }
    public string? BuildingNumber { get; set; }
    public string? PostCode { get; set; }
    public string? AdditionalNumber { get; set; }
    public string? RegionName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
