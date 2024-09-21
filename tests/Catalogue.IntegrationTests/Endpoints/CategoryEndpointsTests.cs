using System.Net;
using System.Text.Json;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.DTOs;
using Catalogue.Application.Interfaces;
using Catalogue.Application.Pagination;
using Catalogue.Domain.Entities;
using Catalogue.IntegrationTests.Fixtures;

namespace Catalogue.IntegrationTests.Endpoints;

[Collection(nameof(CustomWebAppFixture))]
public class CategoryEndpointsTests 
{
    private readonly CustomWebAppFixture _fixture;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public CategoryEndpointsTests(CustomWebAppFixture fixture)
    {
        _fixture = fixture;
        _httpClient = fixture.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7140/api/v1/categories/");
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }


    /// <summary>
    /// Verifies that the 'https://localhost:7140/api/v1/categories/' endpoint
    /// returns a 200 OK status and includes valid pagination metadata when provided
    /// with a valid query string.
    /// </summary>
    [Fact]
    public async Task GetCategories_WithValidQueryString_ShouldReturn200OKAndCategories()
    {
        //Arrange
        int pageNumber = 1;
        int pageSize = 20;
        string queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";

        //Act
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(queryString);
        
        var stream = await httpResponse.Content.ReadAsStreamAsync();

        IPagedList<GetCategoryQueryResponse>? response =
            await JsonSerializer.DeserializeAsync<PagedList<GetCategoryQueryResponse>>
            (
                stream,
                _options
            );

        string? header = httpResponse.Headers.GetValues("X-Pagination").FirstOrDefault();
        PaginationMetadata? metadata = JsonSerializer.Deserialize<PaginationMetadata>(header);

        //Assert
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.NotEmpty(response);
        Assert.Equal(pageSize, response.Count());
        Assert.Equal(pageNumber, metadata.PageCurrent);
        Assert.False(metadata.HasPreviousPage);
        Assert.True(metadata.HasNextPage);
    }

    /// <summary>
    /// Verifies that the 'https://localhost:7140/api/v1/categories/{id}' endpoint
    /// returns 200 OK status codes and the expected category.
    /// </summary>
    [Fact]
    public async Task GetCategoryById_WhenExistsCategoryId_ShouldReturn200OKAndCategory()
    {
        //Arrange
        Category? categoryExpected = _fixture?.DbContext?.Categories.First();

        //Act
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(categoryExpected?.Id.ToString());

        GetCategoryQueryResponse? response =
            await _fixture.ReadHttpResponseAsync<GetCategoryQueryResponse>
            (
                httpResponse,
                 _options
            ); 

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.Equal(categoryExpected?.Id, response.Id);
    }
}