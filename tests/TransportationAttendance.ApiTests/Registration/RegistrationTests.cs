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
    public async Task GetStudentInfo_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/registration/student-info");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetMyRegistration_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/registration/my-registration");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetPendingRegistrations_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/registration/pending");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SubmitRegistration_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();
        var request = new
        {
            districtId = Guid.NewGuid(),
            preferredBusId = Guid.NewGuid(),
            transportType = 1
        };

        // Act
        var response = await PostAsync("/api/v1/registration", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();
        var registrationId = Guid.NewGuid();

        // Act
        var response = await GetAsync($"/api/v1/registration/{registrationId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
