using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TransportationAttendance.Infrastructure.Persistence;
using Xunit;

namespace TransportationAttendance.ApiTests.TestInfrastructure;

public abstract class TestBase : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly CustomWebApplicationFactory Factory;
    private readonly TestAuthHelper _authHelper;

    protected TestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _authHelper = new TestAuthHelper(Client);
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    #region Authentication Helpers

    protected async Task<string> LoginAsAdminAsync()
        => await _authHelper.LoginAsync("admin", "Admin123!");

    protected async Task<string> LoginAsStaffAsync()
        => await _authHelper.LoginAsync("staff", "Staff123!");

    protected async Task<string> LoginAsSupervisorAsync()
        => await _authHelper.LoginAsync("supervisor", "Supervisor123!");

    protected void SetAuthToken(string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    protected void ClearAuthToken()
    {
        Client.DefaultRequestHeaders.Authorization = null;
    }

    #endregion

    #region HTTP Helpers

    protected async Task<HttpResponseMessage> GetAsync(string url, string? token = null)
    {
        if (token != null) SetAuthToken(token);
        return await Client.GetAsync(url);
    }

    protected async Task<HttpResponseMessage> PostAsync<T>(string url, T content, string? token = null)
    {
        if (token != null) SetAuthToken(token);
        return await Client.PostAsJsonAsync(url, content);
    }

    protected async Task<HttpResponseMessage> PutAsync<T>(string url, T content, string? token = null)
    {
        if (token != null) SetAuthToken(token);
        return await Client.PutAsJsonAsync(url, content);
    }

    protected async Task<HttpResponseMessage> DeleteAsync(string url, string? token = null)
    {
        if (token != null) SetAuthToken(token);
        return await Client.DeleteAsync(url);
    }

    protected async Task<T?> GetJsonAsync<T>(string url, string? token = null)
    {
        var response = await GetAsync(url, token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    #endregion

    #region Database Helpers

    protected async Task ResetDatabaseAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TransportationDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    #endregion
}
