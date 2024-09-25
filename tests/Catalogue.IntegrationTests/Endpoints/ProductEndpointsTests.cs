using Catalogue.Application.DTOs;
using Catalogue.Application.Interfaces;
using Catalogue.Application.Pagination;
using Catalogue.Application.Products.Queries.Responses;
using Catalogue.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Catalogue.IntegrationTests.Endpoints
{
    [Collection(nameof(CustomWebAppFixture))]
    public class ProductEndpointsTests
    {
        private readonly CustomWebAppFixture _fixture;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options;

        public ProductEndpointsTests(CustomWebAppFixture fixture)
        {
            _fixture = fixture;
            _httpClient = _fixture.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7140/api/v1/products/");
            _options = new JsonSerializerOptions(){ PropertyNameCaseInsensitive = true };
        }

        /// <summary>
        /// Tests that 'get' request to the 'https://localhost:7140/api/v1/products/' endpoint
        /// returns a 200 OK and the products paginated.
        /// </summary>
        [Fact]
        public async Task GetAllProducts_WhenQueryStringIsValid_ShouldReturn200OKAndProductsPaginated()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;

            string queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";
            string token = _fixture.GenerateToken(_fixture.Admin.Name, _fixture.Admin.Role);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(queryString);

            PaginationMetadata? metadata = _fixture.GetHeaderPagination(httpResponse);

            IPagedList<GetProductQueryResponse>? response = 
                await _fixture.ReadHttpResponseAsync<PagedList<GetProductQueryResponse>>
                (
                    httpResponse,
                     _options
                );
            
            // Assert 
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.NotEmpty(response);
            Assert.Equal(pageNumber, metadata.PageCurrent);
            Assert.Equal(pageSize, metadata.PageSize);
            Assert.Equal(pageSize, metadata.PageSize);
            Assert.False(metadata.HasPreviousPage);
            Assert.True(metadata.HasNextPage);
        }
    }
}