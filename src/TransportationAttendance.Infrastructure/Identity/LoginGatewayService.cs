using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TransportationAttendance.Infrastructure.Identity;

public interface ILoginGatewayService
{
    Task<LoginGatewayResponse?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
}

public class LoginGatewayService : ILoginGatewayService
{
    private readonly HttpClient _httpClient;
    private readonly LoginGatewaySettings _settings;
    private readonly ILogger<LoginGatewayService> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public LoginGatewayService(
        HttpClient httpClient,
        IOptions<LoginGatewaySettings> settings,
        ILogger<LoginGatewayService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<LoginGatewayResponse?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new LoginGatewayRequest
            {
                Username = username,
                Password = password
            };

            _logger.LogInformation("Calling LoginGateway API for user: {Username}", username);

            var response = await _httpClient.PostAsJsonAsync(
                "/api/v1/Auth/login",
                request,
                JsonOptions,
                cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            
            _logger.LogDebug("LoginGateway response status: {StatusCode}", response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("LoginGateway returned error: {StatusCode} - {Content}", 
                    response.StatusCode, content);
                
                var errorResponse = JsonSerializer.Deserialize<LoginGatewayResponse>(content, JsonOptions);
                return errorResponse;
            }

            var result = JsonSerializer.Deserialize<LoginGatewayResponse>(content, JsonOptions);
            
            if (result?.Success == true)
            {
                _logger.LogInformation("LoginGateway authentication successful for user: {Username}", username);
            }
            
            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error calling LoginGateway API");
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON deserialization error from LoginGateway API");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling LoginGateway API");
            return null;
        }
    }
}

public class LoginGatewayRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class LoginGatewayResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
    [JsonPropertyName("data")]
    public LoginGatewayData? Data { get; set; }
    
    [JsonPropertyName("errors")]
    public List<string>? Errors { get; set; }
}

public class LoginGatewayData
{
    [JsonPropertyName("token")]
    public string? Token { get; set; }
    
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
    
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }
    
    [JsonPropertyName("roles")]
    public List<string>? Roles { get; set; }
    
    [JsonPropertyName("full_name_official_ar")]
    public string? FullNameOfficialAr { get; set; }
    
    [JsonPropertyName("full_name_official_en")]
    public string? FullNameOfficialEn { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
    
    [JsonPropertyName("national_id")]
    public string? NationalId { get; set; }
    
    [JsonPropertyName("gender")]
    public string? Gender { get; set; }
    
    [JsonPropertyName("dob")]
    public string? Dob { get; set; }
    
    [JsonPropertyName("user_language")]
    public string? UserLanguage { get; set; }
    
    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }
}
