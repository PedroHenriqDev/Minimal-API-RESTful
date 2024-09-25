using System.Net;
using System.Net.Http.Headers;
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

namespace Catalogue.IntegrationTests.Endpoints;

[Collection(nameof(CustomWebAppFixture))]
public class AuthEndpointsTests : IAsyncLifetime
{
    private readonly CustomWebAppFixture _fixture;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;
    private RegisterUserCommandRequest userRegistered;

    public AuthEndpointsTests(CustomWebAppFixture fixture)
    {
        _fixture = fixture;
        _httpClient = fixture.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7140/api/v1/auth/");
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    /// <summary>
    /// Tests that a 'post' request to the 'https://api/v1/auth/register' endpoint
    /// returns a 201 Created status when provided with a valid request.
    /// </summary>
    [Fact]
    public async Task RegisterUser_WhenValidUser_ShouldReturnStatusCodes201Created()
    {
        //Arrange
        userRegistered = new AutoFaker<RegisterUserCommandRequest>()
        .RuleFor(r => r.Password, f => f.Internet.Password()).Generate();
        userRegistered.Password += "1";

        StringContent content = _fixture.CreateStringContent(userRegistered);

        //Act
        HttpResponseMessage httpResponse = await _httpClient.PostAsync("register", content);

        RegisterUserCommandResponse? response = await _fixture.ReadHttpResponseAsync<RegisterUserCommandResponse>(httpResponse, _options);

        //Assert
        Assert.NotNull(httpResponse);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
    }

    /// <summary>
    /// Tests that a 'post' request to the 'https://api/v1/auth/register' endpoint
    /// returns a 400 Bad Request status when provided invalid request.
    /// </summary> 
    [Fact]
    public async Task RegisterUser_WhenInvalidUser_ShouldReturnStatusCodes400BadRequest()
    {
        //Arrange
        userRegistered = new AutoFaker<RegisterUserCommandRequest>()
            .Ignore(r => r.Name);

        StringContent content = _fixture.CreateStringContent(userRegistered);

        //Act 
        HttpResponseMessage httpResponse = await _httpClient.PostAsync("register", content);

        ErrorResponse? response = await _fixture.ReadHttpResponseAsync<ErrorResponse>(httpResponse, _options);

        Assert.NotNull(httpResponse);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
    }

    /// <summary>
    /// Tests that a 'post' request to the 'https://api/v1/auth/login' endpoint
    /// returns a 200 OK status when a valid login request is provided.
    /// </summary>
    [Fact]
    public async Task Login_WhenValidLogin_ShouldReturnStatusCodes200Ok()
    {
        //Arrange
        var login = new LoginQueryRequest
        {
            Name = userRegistered.Name,
            Password = userRegistered.Password
        };

        StringContent content = _fixture.CreateStringContent(login);

        //Act
        HttpResponseMessage httpResponse = await _httpClient.PostAsync("login", content);

        LoginQueryResponse? response = await _fixture.ReadHttpResponseAsync<LoginQueryResponse>(httpResponse, _options);

        //Assert
        Assert.NotNull(httpResponse);
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Token);
        Assert.Equal(response.User.Name, login.Name);
    }

    /// <summary>
    ///  Tests that a 'post' request to the 'https://api/v1/auth/login' endpoint
    /// returns a 401 Unauthorized status when a invalid login request is provided.
    /// 
    /// </summary>
    [Fact]
    public async Task Login_WhenLoginInvalid_ShouldReturnStatusCodes401Unauthorized()
    {
        // Arrange
        var login = new AutoFaker<LoginQueryRequest>().Generate();

        StringContent content = _fixture.CreateStringContent(login);

        // Act
        HttpResponseMessage? httpResponse = await _httpClient.PostAsync("login", content);

        // Assert
        Assert.NotNull(httpResponse);
        Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
    }

    /// <summary>
    /// Tests that a 'put' request to the 'https://api/v1/auth/role/{id}' endpoint
    /// returns a 200 OK status when a valid role and user request is provided.
    /// <remarks>
    /// Note: This test requires that there is least one user in the database
    /// with the role 'Admin'.
    /// If no such user exists, the test will fail.
    /// </remarks>
    /// </summary>
    [Fact]
    public async Task UpdateRole_WhenRoleAndUserValid_ShouldReturnStatusCodes200Ok()
    {
        // Arrange
        string token = _fixture.GenerateToken(_fixture.Admin.Name, _fixture.Admin.Role);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        User userToUpdate = _fixture.DbContext.Users.First(u => u.Name == userRegistered.Name);

        var request = new UpdateUserRoleCommandRequest
        {
            RoleName = "Admin",
        };

        StringContent content = _fixture.CreateStringContent(request);

        // Act
        HttpResponseMessage? httpResponse = await _httpClient.PutAsync($"role/{userToUpdate.Id}", content);

        UpdateUserRoleCommandResponse? response =
            await _fixture.ReadHttpResponseAsync<UpdateUserRoleCommandResponse>
            (
                httpResponse,
                 _options
            );

        User userUpdated = _fixture.DbContext.Users.First(u => u.Name == userRegistered.Name);

        // Arrange
        Assert.NotNull(httpResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.NotNull(response);
        Assert.NotNull(response.User);
        Assert.Equal(Role.Admin, response?.User.Role);
        Assert.Equal("Admin", response.User.RoleName);
    }

    /// <summary>
    ///  Tests that a 'put' request to the 'https://api/v1/auth/role/{id}' endpoint
    /// returns a 403 Forbidden status when a valid role and invalid user request is provided.
    /// </summary>
    [Fact]
    public async Task UpdateRole_WhenRoleValidAndUserInvalid_ShouldReturnStatusCodes403Forbidden()
    {
        //Arrange
         string token = _fixture.GenerateToken(userRegistered.Name, Role.User);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        User userToUpdate = _fixture.DbContext.Users.First(u => u.Name == userRegistered.Name);

        var request = new UpdateUserRoleCommandRequest
        {
            RoleName = "Admin",
        };

        StringContent content = _fixture.CreateStringContent(request);

        // Act
        HttpResponseMessage? httpResponse = await _httpClient.PutAsync($"role/{userToUpdate.Id}", content);

        // Arrange
        Assert.Equal(HttpStatusCode.Forbidden, httpResponse.StatusCode);
    }

    /// <summary>
    /// Tests that a 'put' request to the 'https://api/v1/auth/update-user' endpoint 
    /// returns 200 OK Status when a valid user request is provided.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task UpdateUser_WhenUserValid_ShouldReturnsStatusCodes200OK()
    {
        string token = _fixture.GenerateToken
        (
            _fixture.UserRandom.Name,
            _fixture.UserRandom.Role
        );

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new AutoFaker<UpdateUserCommandRequest>()
            .Ignore(u => u.Name)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.BirthDate, f => f.Date.Past(60, new DateTime(2010, 1, 1)))
            .Generate();

        StringContent content = _fixture.CreateStringContent(request);

        HttpResponseMessage httpResponse = await _httpClient.PutAsync("update-user", content);
        UpdateUserCommandResponse? response = await _fixture.ReadHttpResponseAsync<UpdateUserCommandResponse>(httpResponse, _options);

        Assert.NotNull(httpResponse);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.NotEqual(request.Name, response.User.Name);
    }

    /// <summary>
    /// Initializes and registers a new user.
    /// </summary>
    public async Task InitializeAsync()
    {
        await RegisterUser_WhenValidUser_ShouldReturnStatusCodes201Created();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}