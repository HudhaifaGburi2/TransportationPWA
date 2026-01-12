using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Health;

public class HealthTests : TestBase
{
    public HealthTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task HealthCheck_ReturnsHealthyStatus()
    {
        // Act
        var response = await GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<HealthResponse>();
        result.Should().NotBeNull();
        result!.Status.Should().Be("Healthy");
        result.Timestamp.Should().NotBe(default);
    }

    [Fact]
    public async Task HealthCheck_WithoutAuthentication_StillWorks()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private record HealthResponse(string Status, DateTime Timestamp);
}
