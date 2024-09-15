using System.Net;
using System.Text;
using System.Text.Json;
using AutoBogus;
using Catalogue.API;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Application.Users.Queries.Requests;
using Catalogue.Application.Users.Queries.Responses;
using Catalogue.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Catalogue.IntegrationTests.Endpoints;

public class AuthEndpointsTests : IClassFixture<CustomWebAppFixture>, IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private const string url = "https://localhost:7140/api/v1/auth/";
    private readonly JsonSerializerOptions _options;
    private RegisterUserCommandRequest userRegistered;

    public AuthEndpointsTests(CustomWebAppFixture app)
    {
        _httpClient = app.CreateClient();
        _options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Verifies that 'RegisterUser' returns a 201 Created status when provided with a valid request.
    /// </summary>
    [Fact]
    public async Task RegisterUser_WhenGivenValidUser_ReturnStatusCodes201Created()
    {
        //Arrange
        userRegistered = new AutoFaker<RegisterUserCommandRequest>()
        .RuleFor(s => s.Password, f => f.Internet.Password()).Generate();
        userRegistered.Password += "1";


        var content = new StringContent
        (
         JsonConvert.SerializeObject(userRegistered),
         System.Text.Encoding.UTF8, "application/json"
        );

        //Act
        HttpResponseMessage response = await _httpClient.PostAsync(url + "register", content);
        Stream responseContentAsStream = await response.Content.ReadAsStreamAsync();
        var userCreated = await JsonSerializer.DeserializeAsync<RegisterUserCommandResponse>(responseContentAsStream, _options);

        //Assert
        Assert.NotNull(response);
        Assert.NotNull(userCreated);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    /// <summary>
    /// Verifies that 'Login' returns a 200 OK status when a valid login request is provided.
    /// </summary>
    [Fact]
    public async Task Login_WhenGivenValidLogin_ReturnStatusCodes200Ok()
    {
        //Arrange
        var login = new LoginQueryRequest
        {
            Name = userRegistered.Name,
            Password = userRegistered.Password
        };

        var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");

        //Act
        HttpResponseMessage response = await _httpClient.PostAsync(url + "login", content);

        Stream responseContentAsStream = await response.Content.ReadAsStreamAsync();
        var loginResponse = await JsonSerializer.DeserializeAsync<LoginQueryResponse>(responseContentAsStream, _options);
        
        //Assert
        Assert.NotNull(response);
        Assert.NotNull(loginResponse);
        Assert.True(loginResponse.Success);
        Assert.NotNull(loginResponse.Token);
        Assert.Equal(loginResponse.User.Name, login.Name);
    }

    /// <summary>
    /// Initializes and registers a new user.
    /// </summary>
    public async Task InitializeAsync()
    {
        await RegisterUser_WhenGivenValidUser_ReturnStatusCodes201Created();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}