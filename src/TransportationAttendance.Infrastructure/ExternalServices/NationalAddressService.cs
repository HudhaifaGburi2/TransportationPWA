using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TransportationAttendance.Application.Common;
using TransportationAttendance.Application.DTOs.NationalAddress;
using TransportationAttendance.Application.Interfaces;

namespace TransportationAttendance.Infrastructure.ExternalServices;

/// <summary>
/// Service for looking up Saudi National Addresses via the official API
/// API: http://apina.address.gov.sa/NationalAddress/NationalAddressByShortAddress/NationalAddressByShortAddress
/// </summary>
public class NationalAddressService : INationalAddressService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NationalAddressService> _logger;
    private const string BaseUrl = "http://apina.address.gov.sa/NationalAddress/NationalAddressByShortAddress/NationalAddressByShortAddress";

    public NationalAddressService(HttpClient httpClient, ILogger<NationalAddressService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<NationalAddressDto>> LookupAddressAsync(string shortAddress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(shortAddress))
        {
            return Result.Failure<NationalAddressDto>("العنوان الوطني المختصر مطلوب.");
        }

        // Validate format: 4 letters + 4 digits
        if (!System.Text.RegularExpressions.Regex.IsMatch(shortAddress, @"^[A-Za-z]{4}\d{4}$"))
        {
            return Result.Failure<NationalAddressDto>("صيغة العنوان الوطني غير صحيحة. يجب أن يكون 4 أحرف + 4 أرقام.");
        }

        try
        {
            var url = $"{BaseUrl}?format=json&language=ar&shortaddress={shortAddress.ToUpper()}";
            _logger.LogInformation("Looking up national address: {ShortAddress}", shortAddress);

            var response = await _httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("National Address API returned status {StatusCode} for {ShortAddress}", 
                    response.StatusCode, shortAddress);
                return Result.Failure<NationalAddressDto>("فشل في الاتصال بخدمة العنوان الوطني. حاول مرة أخرى.");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var apiResponse = JsonSerializer.Deserialize<NationalAddressApiResponse>(content, options);

            if (apiResponse?.Addresses == null || apiResponse.Addresses.Count == 0)
            {
                _logger.LogWarning("No addresses found for {ShortAddress}", shortAddress);
                return Result.Failure<NationalAddressDto>("لم يتم العثور على عنوان مطابق. تأكد من صحة العنوان الوطني المختصر.");
            }

            var address = apiResponse.Addresses[0];
            
            // Build full address string
            var fullAddressParts = new List<string>();
            if (!string.IsNullOrEmpty(address.City)) fullAddressParts.Add(address.City);
            if (!string.IsNullOrEmpty(address.District)) fullAddressParts.Add($"حي {address.District}");
            if (!string.IsNullOrEmpty(address.Street)) fullAddressParts.Add($"شارع {address.Street}");
            if (!string.IsNullOrEmpty(address.BuildingNumber)) fullAddressParts.Add($"مبنى {address.BuildingNumber}");
            if (!string.IsNullOrEmpty(address.PostCode)) fullAddressParts.Add($"الرمز البريدي {address.PostCode}");
            
            var fullAddress = string.Join("، ", fullAddressParts);

            _logger.LogInformation("Successfully resolved address for {ShortAddress}: {FullAddress}", 
                shortAddress, fullAddress);

            return new NationalAddressDto
            {
                Success = true,
                ShortAddress = shortAddress.ToUpper(),
                FullAddress = fullAddress,
                City = address.City,
                District = address.District,
                Street = address.Street,
                BuildingNumber = address.BuildingNumber,
                PostalCode = address.PostCode,
                AdditionalNumber = address.AdditionalNumber,
                Latitude = address.Latitude.HasValue ? (decimal)address.Latitude.Value : null,
                Longitude = address.Longitude.HasValue ? (decimal)address.Longitude.Value : null
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error looking up national address: {ShortAddress}", shortAddress);
            return Result.Failure<NationalAddressDto>("خطأ في الاتصال بخدمة العنوان الوطني. تحقق من اتصالك بالإنترنت.");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for national address response: {ShortAddress}", shortAddress);
            return Result.Failure<NationalAddressDto>("خطأ في معالجة استجابة خدمة العنوان الوطني.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error looking up national address: {ShortAddress}", shortAddress);
            return Result.Failure<NationalAddressDto>("حدث خطأ غير متوقع. حاول مرة أخرى.");
        }
    }
}
