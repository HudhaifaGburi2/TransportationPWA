using System.Net.Http.Json;

namespace TransportationAttendance.ApiTests.TestInfrastructure;

public class TestAuthHelper
{
    private readonly HttpClient _client;
    private readonly Dictionary<string, TokenCache> _tokenCache = new();

    public TestAuthHelper(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        // Check cache first
        if (_tokenCache.TryGetValue(username, out var cached) && !cached.IsExpired)
        {
            return cached.Token;
        }

        // Real login call
        var response = await _client.PostAsJsonAsync("/api/v1/auth/login", new
        {
            username,
            password
        });

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Login failed for {username}: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<LoginApiResponse>();

        if (result?.Data?.Token == null)
        {
            throw new InvalidOperationException($"Login response missing token for {username}");
        }

        // Cache token
        _tokenCache[username] = new TokenCache
        {
            Token = result.Data.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(55)
        };

        return result.Data.Token;
    }

    public void ClearCache()
    {
        _tokenCache.Clear();
    }

    private class TokenCache
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }

    private record LoginApiResponse(bool Success, LoginData? Data, string? Message);
    private record LoginData(string Token, string? RefreshToken, string? UserId, string? Username, string? FullName, List<string>? Roles);
}
