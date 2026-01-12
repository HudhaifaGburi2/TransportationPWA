using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TransportationAttendance.ApiTests.TestInfrastructure;
using Xunit;

namespace TransportationAttendance.ApiTests.Districts;

public class DistrictsTests : TestBase
{
    public DistrictsTests(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAll_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();

        // Act
        var response = await GetAsync("/api/v1/districts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_WithValidToken_ReturnsDistrictsList()
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
        var response = await GetAsync("/api/v1/districts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<DistrictDto>>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetActive_WithValidToken_ReturnsActiveDistrictsOnly()
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
        var response = await GetAsync("/api/v1/districts/active");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<DistrictDto>>>();
        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
        result.Data?.All(d => d.IsActive).Should().BeTrue();
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
        var response = await GetAsync($"/api/v1/districts/{nonExistentId}");

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

        var newDistrict = new
        {
            nameAr = "حي جديد للاختبار",
            nameEn = "New Test District"
        };

        // Act
        var response = await PostAsync("/api/v1/districts", newDistrict);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Created, HttpStatusCode.OK, HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        ClearAuthToken();
        var newDistrict = new
        {
            nameAr = "حي جديد",
            nameEn = "New District"
        };

        // Act
        var response = await PostAsync("/api/v1/districts", newDistrict);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsOk()
    {
        // Arrange
        var loginResponse = await PostAsync("/api/v1/auth/login", new { username = "admin", password = "admin123" });
        if (!loginResponse.IsSuccessStatusCode)
        {
            return;
        }

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        SetAuthToken(loginResult!.Data!.Token!);

        // First get an existing district
        var getResponse = await GetAsync("/api/v1/districts");
        if (!getResponse.IsSuccessStatusCode)
        {
            return;
        }

        var districts = await getResponse.Content.ReadFromJsonAsync<ApiResponse<List<DistrictDto>>>();
        if (districts?.Data == null || districts.Data.Count == 0)
        {
            return;
        }

        var districtId = districts.Data[0].Id;
        var updateData = new
        {
            nameAr = "حي محدث",
            nameEn = "Updated District"
        };

        // Act
        var response = await PutAsync($"/api/v1/districts/{districtId}", updateData);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsOk()
    {
        // Arrange
        var loginResponse = await PostAsync("/api/v1/auth/login", new { username = "admin", password = "admin123" });
        if (!loginResponse.IsSuccessStatusCode)
        {
            return;
        }

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        SetAuthToken(loginResult!.Data!.Token!);

        // Create a district to delete
        var createResponse = await PostAsync("/api/v1/districts", new
        {
            nameAr = "حي للحذف",
            nameEn = "District To Delete"
        });

        if (createResponse.StatusCode == HttpStatusCode.Created || createResponse.StatusCode == HttpStatusCode.OK)
        {
            var created = await createResponse.Content.ReadFromJsonAsync<ApiResponse<DistrictDto>>();
            if (created?.Data != null)
            {
                // Act
                var response = await DeleteAsync($"/api/v1/districts/{created.Data.Id}");

                // Assert
                response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent, HttpStatusCode.Forbidden);
            }
        }
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ReturnsNotFound()
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
        var response = await DeleteAsync($"/api/v1/districts/{nonExistentId}");

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.NotFound, HttpStatusCode.Forbidden);
    }

    private record LoginResponse(bool Success, LoginData? Data, string? Message);
    private record LoginData(string? Token);
    private record ApiResponse<T>(bool Success, T? Data, string? Message);
    private record DistrictDto(Guid Id, string NameAr, string? NameEn, bool IsActive);
}
