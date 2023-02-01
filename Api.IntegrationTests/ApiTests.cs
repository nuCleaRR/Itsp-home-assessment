using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.IntegrationTests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;
    
    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetAll_WhenNoAuthorization_ReturnsUnauthorized()
    {
        var userResponse = await _httpClient.GetAsync("/users/get-all");

        userResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await userResponse.Content.ReadAsStringAsync()).Should().BeEmpty();
        
        var moviesResponse = await _httpClient.GetAsync("/movies/get-all");

        moviesResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await moviesResponse.Content.ReadAsStringAsync()).Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAll_WhenHasAdminAuthorization_ReturnsOk()
    {
        HttpClientHelpers.AuthorizeAdmin(_httpClient);
        
        var usersResponse = await _httpClient.GetAsync("/users/get-all");

        usersResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        (await usersResponse.Content.ReadAsStringAsync()).Should().NotBeEmpty();
        
        var adminResponse = await _httpClient.GetAsync("/movies/get-all");

        adminResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        (await adminResponse.Content.ReadAsStringAsync()).Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task GetAll_WhenHasUserAuthorization_ReturnsForbidden()
    {
        HttpClientHelpers.AuthorizeUser(_httpClient);

        var usersResponse = await _httpClient.GetAsync("/users/get-all");

        usersResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await usersResponse.Content.ReadAsStringAsync()).Should().BeEmpty();
        
        var adminResponse = await _httpClient.GetAsync("/movies/get-all");

        adminResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await adminResponse.Content.ReadAsStringAsync()).Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetUserById_WhenHasUserAuthorization_GetsAllUserDetails()
    {
        var user = await GetRandomUser();
        
        HttpClientHelpers.AuthorizeUser(_httpClient);
        
        var userResponse = await _httpClient.GetAsync($"/users/{user.Id}");

        var userModel = await userResponse.Content.ReadFromJsonAsync<User>();
        
        userResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        userModel.Should().NotBeNull();
        userModel.Id.Should().Be(user.Id);
    }
    
    [Fact]
    public async Task GetUserById_WhenHasUserAuthorization_GetsDetailsOfAnyUser()
    {
        var userId = (await GetRandomUser()).Id;
        
        HttpClientHelpers.AuthorizeUser(_httpClient);
        
        var userResponse = await _httpClient.GetAsync($"/users/{userId}");

        var userModel = await userResponse.Content.ReadFromJsonAsync<User>();
        
        userResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        userModel.Should().NotBeNull();
        userModel.Id.Should().Be(userId);
    }
    
    [Fact]
    public async Task PutUser_WhenHasUserAuthorization_ChangesNotOwnDetails_ReturnsForbidden()
    {
        var user = await GetRandomUser();
        var modifiedUser = user with { Name = "More fancy name" };
        
        HttpClientHelpers.AuthorizeUser(_httpClient);

        var userResponse = await _httpClient.PutAsync($"/users/", JsonContent.Create(modifiedUser));

        userResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await userResponse.Content.ReadAsStringAsync()).Should().BeEmpty();
    }
    
    [Fact]
    public async Task PutUser_WhenHasUserAuthorization_ChangesOwnDetails_ReturnsOk()
    {
        HttpClientHelpers.AuthorizeAdmin(_httpClient);

        var allUsersResponse = await _httpClient.GetFromJsonAsync<List<User>>("/users/get-all");
        var loggedUser = allUsersResponse.First(x => x.Email == HttpClientHelpers.EmailOfAthentificatedUser);
        
        var modifiedUser = loggedUser with { Name = "Even more fancy name" };
        
        HttpClientHelpers.AuthorizeUser(_httpClient);

        var userResponse = await _httpClient.PutAsync($"/users/", JsonContent.Create(modifiedUser));

        userResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var userModel = await userResponse.Content.ReadFromJsonAsync<User>();
        userModel.Name.Should().Be(modifiedUser.Name);
        
        // let's do double check
        var userFromDb = await _httpClient.GetFromJsonAsync<User>($"/users/{modifiedUser.Id}");
        userFromDb.Name.Should().Be(modifiedUser.Name);
    }

    private async Task<User> GetRandomUser()
    {
        HttpClientHelpers.AuthorizeAdmin(_httpClient);

        var allUsersResponse = await _httpClient.GetFromJsonAsync<List<User>>("/users/get-all");
        var user = allUsersResponse
            .Where(x => x.Email != HttpClientHelpers.EmailOfAthentificatedUser)
            .ToList()
            .PickRandom();

        return user;
    }
}