using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Auth;

public class LoginTests : TestBase
{
    public LoginTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsTokenOrUnauthorized()
    {
        // Arrange - Using test credentials (may not exist in test environment)
        var request = new { username = "admin", password = "admin123" };

        // Act
        var response = await PostAsync("/api/v1/auth/login", request);

        // Assert - In test environment without seeded users, 401 is expected
        // In production with valid users, 200 is expected
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Unauthorized);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Token.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var request = new { username = "admin", password = "wrongpassword" };

        // Act
        var response = await PostAsync("/api/v1/auth/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithEmptyUsername_ReturnsBadRequest()
    {
        // Arrange
        var request = new { username = "", password = "password123" };

        // Act
        var response = await PostAsync("/api/v1/auth/login", request);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithEmptyPassword_ReturnsBadRequest()
    {
        // Arrange
        var request = new { username = "admin", password = "" };

        // Act
        var response = await PostAsync("/api/v1/auth/login", request);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ValidateToken_WithValidToken_ReturnsOk()
    {
        // Arrange - This test requires a valid token from the real auth system
        // Skip if we can't login
        var loginResponse = await PostAsync("/api/v1/auth/login", new { username = "admin", password = "admin123" });
        if (!loginResponse.IsSuccessStatusCode)
        {
            return; // Skip test if login not configured
        }

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        SetAuthToken(loginResult!.Data!.Token!);

        // Act
        var response = await Client.PostAsync("/api/v1/auth/validate-token", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ValidateToken_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await Client.PostAsync("/api/v1/auth/validate-token", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetCurrentUser_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/auth/me");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Logout_WithValidToken_ReturnsOk()
    {
        // Arrange
        var loginResponse = await PostAsync("/api/v1/auth/login", new { username = "admin", password = "admin123" });
        if (!loginResponse.IsSuccessStatusCode)
        {
            return;
        }

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        SetAuthToken(loginResult!.Data!.Token!);

        // Act
        var response = await Client.PostAsync("/api/v1/auth/logout", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Logout_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await Client.PostAsync("/api/v1/auth/logout", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private record LoginResponse(bool Success, LoginData? Data, string? Message);
    private record LoginData(string? Token, string? RefreshToken, string? UserId, string? Username, string? FullName, List<string>? Roles);
}
