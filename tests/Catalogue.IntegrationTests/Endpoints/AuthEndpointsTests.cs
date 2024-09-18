using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoBogus;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Application.Users.Queries.Requests;
using Catalogue.Application.Users.Queries.Responses;
using Catalogue.Domain.Entities;
using Catalogue.Domain.Enums;
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
        .RuleFor(r => r.Password, f => f.Internet.Password()).Generate();
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
            .Ignore(r => r.Name);

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
    /// 
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
    /// Verifies that 'Update Role' returns a 200 OK status when a valid role and user request is provided.
    /// <remarks>
    /// Note: This test requires that there is least one user in the database with the role 'Admin'.
    /// If no such user exists, the test will fail.
    /// </remarks>
    /// </summary>
    [Fact]
    public async Task UpdateRole_GivenRoleAndUserValid_ReturnStatusCodes200Ok()
    {
        // Arrange
        var login = new LoginQueryRequest
        {
            Name = _fixture.Admin.Name,
            Password = _fixture.Admin.Password
        };

        var contentLogin = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, mediaType);
        HttpResponseMessage? httpResponseLogin = await _httpClient.PostAsync(url + "login", contentLogin);
        LoginQueryResponse? responseLogin = await JsonSerializer.DeserializeAsync<LoginQueryResponse>
        (
            await httpResponseLogin.Content.ReadAsStreamAsync(),
             _options
        );

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseLogin.Token); 

        User userToUpdate = _fixture.DbContext.Users.First(u => u.Name == userRegistered.Name);

        var requestRole = new UpdateUserRoleCommandRequest
        {
            RoleName = "Admin",
        };

        var contentRole = new StringContent
        (
            JsonSerializer.Serialize(requestRole),
            Encoding.UTF8,
            mediaType
        );

        // Act
        HttpResponseMessage? httpResponseRole = await _httpClient.PutAsync(url + $"role/{userToUpdate.Id}", contentRole);

        UpdateUserRoleCommandResponse? responseRole = await JsonSerializer.DeserializeAsync<UpdateUserRoleCommandResponse>
        (
            await httpResponseRole.Content.ReadAsStreamAsync(),
             _options
        );
        User userUpdated = _fixture.DbContext.Users.First(u => u.Name == userRegistered.Name);

        // Arrange
        Assert.NotNull(httpResponseRole);
        Assert.Equal(HttpStatusCode.OK, httpResponseRole.StatusCode);
        Assert.NotNull(responseRole);
        Assert.NotNull(responseRole.User);
        Assert.Equal( Role.Admin, responseRole?.User.Role);
        Assert.Equal("Admin", responseRole.User.RoleName);
    }

    /// <summary>
    /// Verifies that 'Update Role' returns a 403 Forbidden status when a valid role and invalid user request is provided.
    /// </summary>
    [Fact]
    public async Task UpdateRole_GivenRoleValidAndUserInvalid_ReturnStatusCodes403Forbidden()
    {
        // Arrange
        var login = new LoginQueryRequest
        {
            Name = userRegistered.Name,
            Password = userRegistered.Password
        };

        var contentLogin = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, mediaType);
        HttpResponseMessage? httpResponseLogin = await _httpClient.PostAsync(url + "login", contentLogin);
        LoginQueryResponse? responseLogin = await JsonSerializer.DeserializeAsync<LoginQueryResponse>
        (
            await httpResponseLogin.Content.ReadAsStreamAsync(),
             _options
        );

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseLogin.Token); 

        User userToUpdate = _fixture.DbContext.Users.First(u => u.Name == userRegistered.Name);

        var requestRole = new UpdateUserRoleCommandRequest
        {
            RoleName = "Admin",
        };

        var contentRole = new StringContent
        (
            JsonSerializer.Serialize(requestRole),
            Encoding.UTF8,
            mediaType
        );

        // Act
        HttpResponseMessage? httpResponseRole = await _httpClient.PutAsync(url + $"role/{userToUpdate.Id}", contentRole);

        // Arrange
        Assert.Equal(HttpStatusCode.Forbidden, httpResponseRole.StatusCode);
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