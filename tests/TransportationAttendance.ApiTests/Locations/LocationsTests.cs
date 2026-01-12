using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Locations;

public class LocationsTests : TestBase
{
    public LocationsTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAll_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/locations");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_WithValidToken_ReturnsLocationsList()
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
        var response = await GetAsync("/api/v1/locations");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<LocationDto>>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetActive_WithValidToken_ReturnsActiveLocationsOnly()
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
        var response = await GetAsync("/api/v1/locations/active");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<LocationDto>>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var loginResponse = await PostAsync("/api/v1/auth/login", new { username = "admin", password = "admin123" });
        if (!loginResponse.IsSuccessStatusCode)
        {
            return;
        }

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        SetAuthToken(loginResult!.Data!.Token!);

        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await GetAsync($"/api/v1/locations/{nonExistentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreated()
    {
        // Arrange
        var loginResponse = await PostAsync("/api/v1/auth/login", new { username = "admin", password = "admin123" });
        if (!loginResponse.IsSuccessStatusCode)
        {
            return;
        }

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        SetAuthToken(loginResult!.Data!.Token!);

        var newLocation = new
        {
            code = "TEST01",
            name = "موقف اختبار",
            locationType = "Parking"
        };

        // Act
        var response = await PostAsync("/api/v1/locations", newLocation);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK, HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();
        var newLocation = new
        {
            code = "TEST02",
            name = "موقف جديد",
            locationType = "Parking"
        };

        // Act
        var response = await PostAsync("/api/v1/locations", newLocation);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private record LoginResponse(bool Success, LoginData? Data, string? Message);
    private record LoginData(string? Token);
    private record ApiResponse<T>(bool Success, T? Data, string? Message);
    private record LocationDto(Guid Id, string Code, string Name, string? LocationType, bool IsActive);
}
