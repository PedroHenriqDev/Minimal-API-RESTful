using System.Net;
using System.Text.Json;
using AutoBogus;
using Catalogue.Application.Categories.Commands.Requests;
using Catalogue.Application.Categories.Commands.Responses;
using Catalogue.Application.Categories.Queries.Responses;
using Catalogue.Application.DTOs;
using Catalogue.Application.Interfaces;
using Catalogue.Application.Pagination;
using Catalogue.Domain.Entities;
using Catalogue.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;

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
    ///Tests that a 'get' request to the 'https://api/v1/categories/' endpoint
    /// returns a 200 OK status and includes valid pagination metadata when provided
    /// with a valid query string.
    /// </summary>
    [Fact]
    public async Task GetCategories_WhenValidQueryString_ShouldReturn200OKAndCategories()
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

        PaginationMetadata? metadata = _fixture.GetHeaderPagination(httpResponse); 

        //Assert
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.NotEmpty(response);
        Assert.Equal(pageSize, response.Count());
        Assert.Equal(pageNumber, metadata.PageCurrent);
        Assert.False(metadata.HasPreviousPage);
        Assert.True(metadata.HasNextPage);
    }

    /// <summary>
    /// Tests that a 'get' request to the 'https://api/v1/categories/{id}' endpoint
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
    
    /// <summary>
    /// Tests that a 'get' request to the 'https://api/v1/categories/products' endpoint
    /// returns 200 OK status codes and the categories with products.
    /// </summary>
    [Fact]
    public async Task GetCategoriesWithProducts_WhenValidQueryString_ShouldReturn200OKAndCategoriesWithProducts()
    {
        //Arrange
        int pageNumber = 1;
        int pageSize = 15;
        string queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";

        //Act
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"products/{queryString}"); 

        IPagedList<GetCategoryWithProdsQueryResponse>? response =
         await _fixture.ReadHttpResponseAsync<PagedList<GetCategoryWithProdsQueryResponse>>
         (
            httpResponse,
             _options
         );
        
        PaginationMetadata? metadata = _fixture.GetHeaderPagination(httpResponse);

        //Assert
        Assert.NotEmpty(response.Select(c => c.Products));
        Assert.True(response?.Any(c => c.Products != null));
        Assert.Equal(metadata.PageCurrent, pageNumber);
        Assert.Equal(metadata.PageSize, pageSize);
        Assert.False(metadata.HasPreviousPage);
        Assert.True(metadata.HasNextPage);
    }

    /// <summary>
    /// Tests that a 'get' request to the https://api/categories/products/{id} endpoint returns
    /// 200 OK and category with products.
    /// </summary>
    [Fact]
    public async Task GetCategoryWithProducts_WhenExistsCategory_ShouldReturn200OKAndCategoryWithProductsExpected()
    {
        //Arrange
        Category categoryExpected = await _fixture.DbContext.Categories
            .FirstAsync(c => c.Products != null && c.Products.Any());

        //Act
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"products/{categoryExpected.Id}");

        GetCategoryWithProdsQueryResponse? response = 
            await _fixture.ReadHttpResponseAsync<GetCategoryWithProdsQueryResponse>
            (
                httpResponse,
                 _options
            );

        //Assert   
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.Equal(response.Id, categoryExpected.Id);
        Assert.NotEmpty(response.Products);
        Assert.Equal(categoryExpected.Products.Count(), response.Products.Count());
    }

    /// <summary>
    /// Tests that a 'post' request to the https://api/categories/ endpoint returns 201 created when
    /// request is valid. 
    /// </summary>
    [Fact]
    public async Task PostCategory_WhenCategoryValid_ShouldReturn201Created()
    {
        //Arrange
        var request = new AutoFaker<CreateCategoryCommandRequest>()
            .Ignore(c => c.CreatedAt)
            .Generate();

        StringContent content = _fixture.CreateStringContent(request);

        //Act
        HttpResponseMessage httpResponse = await _httpClient.PostAsync("", content: content);

        CreateCategoryCommandResponse? response =
            await _fixture.ReadHttpResponseAsync<CreateCategoryCommandResponse>
            (
                httpResponse,
                 _options
            );

        //Assert
        Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
        Assert.NotNull(response);
        Assert.Equal(request.Name, response.Name);
    }
}