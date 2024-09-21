using System.Net;
using System.Text.Json;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.DTOs;
using Catalogue.Application.Interfaces;
using Catalogue.Application.Pagination;
using Catalogue.IntegrationTests.Fixtures;
using NuGet.Frameworks;

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
    /// Verifies that the 'Get Categories' endpoint returns a 200 OK status 
    /// and includes valid pagination metadata when provided with a valid query string.
    /// </summary>
    [Fact]
    public async Task GetCategories_WhenGivenValidQueryString_ReturnStatus200OK()
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
}