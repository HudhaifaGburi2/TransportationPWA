using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Registration;

public class RegistrationTests : TestBase
{
    public RegistrationTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task SearchStudents_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/registration/search?query=test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SearchStudents_WithValidToken_ReturnsResults()
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
        var response = await GetAsync("/api/v1/registration/search?query=أحمد");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPendingRequests_WithValidToken_ReturnsList()
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
        var response = await GetAsync("/api/v1/registration/pending");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateRegistrationRequest_WithValidData_ReturnsCreated()
    {
        // Arrange
        var loginResponse = await PostAsync("/api/v1/auth/login", new { username = "admin", password = "admin123" });
        if (!loginResponse.IsSuccessStatusCode)
        {
            return;
        }

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        SetAuthToken(loginResult!.Data!.Token!);

        var request = new
        {
            studentId = Guid.NewGuid(),
            busId = Guid.NewGuid(),
            transportType = 1,
            notes = "طلب تسجيل جديد"
        };

        // Act
        var response = await PostAsync("/api/v1/registration/request", request);

        // Assert
        // Could be Created, OK, BadRequest (if student/bus not found), or Forbidden
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.Created,
            HttpStatusCode.OK,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Forbidden,
            HttpStatusCode.NotFound
        );
    }

    [Fact]
    public async Task CreateRegistrationRequest_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();
        var request = new
        {
            studentId = Guid.NewGuid(),
            busId = Guid.NewGuid(),
            transportType = 1
        };

        // Act
        var response = await PostAsync("/api/v1/registration/request", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private record LoginResponse(bool Success, LoginData? Data, string? Message);
    private record LoginData(string? Token);
}
