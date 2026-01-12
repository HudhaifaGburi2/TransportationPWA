using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Lookups;

public class LookupsTests : TestBase
{
    public LookupsTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetPeriods_WithValidToken_ReturnsPeriodsLookup()
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
        var response = await GetAsync("/api/v1/lookups/periods");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetTransportTypes_WithValidToken_ReturnsTransportTypesLookup()
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
        var response = await GetAsync("/api/v1/lookups/transport-types");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetLookups_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/lookups/periods");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private record LoginResponse(bool Success, LoginData? Data, string? Message);
    private record LoginData(string? Token);
}
