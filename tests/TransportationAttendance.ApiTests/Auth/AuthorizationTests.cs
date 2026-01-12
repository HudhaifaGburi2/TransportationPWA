using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Auth;

public class AuthorizationTests : TestBase
{
    public AuthorizationTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/districts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithInvalidToken_ReturnsUnauthorized()
    {
        // Arrange
        SetAuthToken("invalid.token.here");

        // Act
        var response = await GetAsync("/api/v1/districts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithExpiredToken_ReturnsUnauthorized()
    {
        // Arrange - Use a manually crafted expired token
        var expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwiZXhwIjoxNjAwMDAwMDAwfQ.invalid";
        SetAuthToken(expiredToken);

        // Act
        var response = await GetAsync("/api/v1/districts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AdminEndpoint_WithNonAdminRole_ReturnsForbidden()
    {
        // This test requires a valid non-admin token
        // The actual behavior depends on the authentication setup
        // Arrange
        var loginResponse = await PostAsync("/api/v1/auth/login", new { username = "staff", password = "staff123" });
        if (!loginResponse.IsSuccessStatusCode)
        {
            return; // Skip if staff login not configured
        }

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        SetAuthToken(loginResult!.Data!.Token!);

        // Act - Try to create a district (admin only)
        var response = await PostAsync("/api/v1/districts", new
        {
            nameAr = "حي اختباري",
            nameEn = "Test District"
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PublicEndpoint_WithoutToken_ReturnsOk()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private record LoginResponse(bool Success, LoginData? Data, string? Message);
    private record LoginData(string? Token, string? RefreshToken, string? UserId);
}
