using System.Net;
using System.Text;
using System.Text.Json;
using AutoBogus;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Exceptions;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Application.Users.Queries.Requests;
using Catalogue.Application.Users.Queries.Responses;
using Catalogue.IntegrationTests.Fixtures;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Catalogue.IntegrationTests.Endpoints;

[Collection(nameof(CustomWebAppFixture))]
public class AuthEndpointsTests : IAsyncLifetime
{
    private readonly CustomWebAppFixture _fixture;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private const string url = "https://localhost:7140/api/v1/auth/";
    private const string mediaType = "application/json";
    private RegisterUserCommandRequest userRegistered;
    
    public AuthEndpointsTests(CustomWebAppFixture app, CustomWebAppFixture fixture)
    {
        _fixture = fixture;
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
        .RuleFor(u => u.Password, f => f.Internet.Password()).Generate();
        userRegistered.Password += "1";

        var content = new StringContent
        (
            JsonConvert.SerializeObject(userRegistered),
            Encoding.UTF8,
            mediaType
        );

        //Act
        HttpResponseMessage response = await _httpClient.PostAsync(url + "register", content);
        Stream contentStream = await response.Content.ReadAsStreamAsync();
        var userCreated = await JsonSerializer.DeserializeAsync<RegisterUserCommandResponse>
        (
            contentStream,
             _options
        );

        //Assert
        Assert.NotNull(response);
        Assert.NotNull(userCreated);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    /// <summary>
    /// Verifies that 'RegisterUser' returns a 400 Bad Request status when provided invalid request.
    /// </summary> 
    [Fact]
    public async Task RegisterUser_WhenGivenInvalidUser_ReturnStatusCodes400BadRequest()
    {
        //Arrange
        userRegistered = new AutoFaker<RegisterUserCommandRequest>()
            .Ignore(u => u.Name);

        var content = new StringContent
        (
           JsonSerializer.Serialize(userRegistered),
           Encoding.UTF8,
           mediaType
        );

        //Act 
        HttpResponseMessage? httpResponse = await _httpClient.PostAsync(url + "register", content);

        Stream contentStream = await httpResponse.Content.ReadAsStreamAsync();
        ErrorResponse? response = await JsonSerializer.DeserializeAsync<ErrorResponse>(contentStream, _options);

        Assert.NotNull(httpResponse);
        Assert.NotNull(contentStream);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
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

        var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, mediaType);

        //Act
        HttpResponseMessage httpResponse = await _httpClient.PostAsync(url + "login", content);

        Stream contentStream = await httpResponse.Content.ReadAsStreamAsync();
        LoginQueryResponse? loginResponse = await JsonSerializer.DeserializeAsync<LoginQueryResponse>(contentStream, _options);
        
        //Assert
        Assert.NotNull(httpResponse);
        Assert.NotNull(loginResponse);
        Assert.True(loginResponse.Success);
        Assert.NotNull(loginResponse.Token);
        Assert.Equal(loginResponse.User.Name, login.Name);
    }

    /// <summary>
    /// Verifies that 'Login' returns a 401 Unauthorized status when a invalid login request is provided.
    /// </summary>
    [Fact]
    public async Task Login_GivenLoginInvalid_ReturnStatusCodes401Unauthorized()
    {
        // Arrange
        var login = new AutoFaker<LoginQueryRequest>().Generate();

        var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, mediaType);

        // Act
        HttpResponseMessage? httpResponse = await _httpClient.PostAsync(url + "login", content);
        
        // Assert
        Assert.NotNull(httpResponse);
        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
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