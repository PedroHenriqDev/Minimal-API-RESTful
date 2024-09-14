using System.Net;
using System.Text.Json;
using AutoBogus;
using Catalogue.API;
using Catalogue.Application.Users.Commands.Requests;
using Catalogue.Application.Users.Commands.Responses;
using Catalogue.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Catalogue.IntegrationTests.Endpoints;

public class AuthEndpointsTests : IClassFixture<WebApplicationFactory<Progam>>
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public AuthEndpointsTests(WebApplicationFactory<Progam> factory)
    {
        _httpClient = factory.CreateClient();
        _options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
    }

    [Fact]
    public async Task RegisterUser_WhenGivenValidUser_ReturnStatusCodes201Created()
    {
        //Arrange
        var user = new AutoFaker<RegisterUserCommandRequest>().Generate();
        user.Password += "1";

        var content = new StringContent
        (
         JsonConvert.SerializeObject(user),
         System.Text.Encoding.UTF8, "application/json"
        );

        string endpointUrl = "https://localhost:7140/api/v1/auth/register";

        //Act
        HttpResponseMessage response = await _httpClient.PostAsync(endpointUrl, content);
        Stream responseAsStream = await response.Content.ReadAsStreamAsync();
        var userCreated = await JsonSerializer.DeserializeAsync<RegisterUserCommandResponse>(responseAsStream, _options);

        //Assert
        Assert.NotNull(response);
        Assert.NotNull(userCreated);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}