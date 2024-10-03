using AutoBogus;
using Catalogue.Application.DTOs;
using Catalogue.Application.DTOs.Responses;
using Catalogue.Application.Interfaces;
using Catalogue.Application.Pagination;
using Catalogue.Application.Products.Commands.Requests;
using Catalogue.Application.Products.Commands.Responses;
using Catalogue.Application.Products.Queries.Responses;
using Catalogue.Domain.Entities;
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
        /// Tests that 'get' request to the 'https://api/v1/products/' endpoint
        /// returns a 200 OK status code response and the products paginated.
        /// </summary>
        [Fact]
        public async Task GetAllProducts_WhenQueryStringIsValid_ShouldReturn200OKAndProductsPaginated()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;

            string queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";
            string token = _fixture.GenerateTokenAdmin();
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

        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/' endpoint when the
        /// token was not sent returns a 401 Unathorized status code.
        /// </summary>
        [Fact]
        public async Task GetAllProducts_WhenNotAuthorized_ShouldReturn401Unauthorized()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            string queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";

            // Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(queryString);

            // Assert 
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }

        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/{id}' endpoint when product
        /// id exists, should returns a 200 OK status code response the expected products.
        /// </summary>
        [Fact]
        public async Task GetByIdProduct_WhenProductIdExists_ShouldReturn200OKAndProductExpected()
        {
            // Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Guid id = _fixture.DbContext.Products.First().Id;

            // Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(id.ToString());

            GetProductQueryResponse? response = 
                await _fixture.ReadHttpResponseAsync<GetProductQueryResponse>
                (
                    httpResponse,
                     _options
                );
            
            // Assert 
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(id, response.Id);
        }

        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/{id}' endpoint when the token
        /// was not sent, should returns a 401 Unauthorized status code response.
        /// </summary>
        [Fact]
        public async Task GetByIdProduct_WhenNotAuthorized_ShouldReturn401Unauthorized()
        {
            // Arrange
            Guid id = _fixture.DbContext.Products.First().Id;

            // Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(id.ToString());

            // Assert 
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }

        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/{id}' endpoint when product
        /// id not exists, should returns a 404 Not Found status code response.
        /// </summary>
        [Fact]
        public async Task GetByIdProduct_WhenProductIdNotExists_ShouldReturn404NotFound()
        {
            // Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Guid id = Guid.NewGuid();

            // Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync(id.ToString());

            ErrorResponse? response = 
                await _fixture.ReadHttpResponseAsync<ErrorResponse>
                (
                    httpResponse,
                     _options
                );
            
            // Assert 
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.NotNull(response);
        }

        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/category/{id}' endpoint when the token
        /// was not sent, should returns a 401 Unauthorized status code response.
        /// </summary>
        [Fact]
        public async Task GetByIdProductWithCategory_WhenNotAuthorized_ShouldReturn401Unauthorized()
        {
            //Arrange
            Product productExpected = _fixture.DbContext.Products.First();

            //Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{productExpected.Id}/category");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }
        
        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/category/{id}' endpoint when product
        /// id exists, should returns a 200 OK status code response and product with your category.
        /// </summary>
        [Fact]
        public async Task GetByIdProductWithCategory_WhenProductIdExists_ShouldReturn200OK()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            Product productExpected = _fixture.DbContext.Products.First();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{productExpected.Id}/category");
            GetProductWithCatQueryResponse? response =
                await _fixture.ReadHttpResponseAsync<GetProductWithCatQueryResponse>
                (
                    httpResponse,
                     _options
                );

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.NotNull(response);
            Assert.NotNull(response.Category);
            Assert.Equal(productExpected.Id, response.Id);
            Assert.Equal(productExpected.Category.Name, response.Category.Name);
        }

        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/category/{id}' endpoint when product
        /// id exists, should returns a 404 Not Found status code response.
        /// </summary>
        [Fact]
        public async Task GetByIdProductWithCategory_WhenProductIdNotExists_ShouldReturn404NotFound()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            Guid id = Guid.NewGuid();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync($"{id}/category");

            ErrorResponse? response =
                await _fixture.ReadHttpResponseAsync<ErrorResponse>
                (
                    httpResponse,
                     _options
                );

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.NotNull(response);
        }

        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/category' endpoint when exists
        /// products, should returns a 200 OK status code response and products with your associated categories.
        /// </summary>
        [Fact]
        public async Task GetProductsWithCategories_WhenQueryStringValid_ShouldReturn200OK()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            int pageNumber = 1;
            int pageSize = 10;

            string queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync("category" + queryString);
            
            IPagedList<GetProductWithCatQueryResponse>? response =
                await _fixture.ReadHttpResponseAsync<PagedList<GetProductWithCatQueryResponse>>
                (
                    httpResponse,
                     _options
                );

            PaginationMetadata? metadata = _fixture.GetHeaderPagination(httpResponse);

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(response.Count(), response.Select(p => p.Category != null).Count());
            Assert.Equal(pageNumber, metadata.PageCurrent);
            Assert.Equal(pageSize, metadata.PageSize);
            Assert.False(metadata.HasPreviousPage);
            Assert.True(metadata.HasNextPage);
        }

        
        /// <summary>
        /// Tests that 'get' request to the 'https://api/v1/products/category' endpoint when the token was not sent,
        /// should returns a 401 Unauthorized status code response.
        /// </summary>
        [Fact]
        public async Task GetProductsWithCategories_WhenNotAuthorized_ShouldReturn401Unauthorized()
        {
            //Arrange
            int pageNumber = 1;
            int pageSize = 10;
            string queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}";

            //Act
            HttpResponseMessage httpResponse = await _httpClient.GetAsync("category" + queryString);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }

        /// <summary>
        /// Tests that 'post' request to the 'https://api/v1/products' endpoint when product 
        /// is valid, should returns a 201 Created status code response.
        /// </summary>
        [Fact]
        public async Task PostProduct_WhenProductIsValid_ShouldReturn201Created()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateProductCommandRequest product = new AutoFaker<CreateProductCommandRequest>()
            .Ignore(p => p.CategoryId)
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url())
            .RuleFor(p => p.CategoryId, f => f
                .PickRandom(_fixture.DbContext.Categories
                    .Select(c => c.Id)
                    .ToList())).Generate();

            StringContent content = _fixture.CreateStringContent(product);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.PostAsync("", content);

            CreateProductCommandResponse? response =
                await _fixture.ReadHttpResponseAsync<CreateProductCommandResponse>(httpResponse, _options);

            //Assert
            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(product.Name, response.Name);
        }

        
        /// <summary>
        /// Tests that 'post' request to the 'https://api/v1/products' endpoint when product 
        /// is invalid, should returns a 400 Bad Request status code response.
        /// </summary>
        [Fact]
        public async Task PostProduct_WhenProductIsInvalid_ShouldReturn400BadRequest()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateProductCommandRequest product = new AutoFaker<CreateProductCommandRequest>()
            .Ignore(p => p.CategoryId)
            .Ignore(p => p.Name)
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url())
            .RuleFor(p => p.CategoryId, f => f
                .PickRandom(_fixture.DbContext.Categories
                    .Select(c => c.Id)
                    .ToList())).Generate();

            StringContent content = _fixture.CreateStringContent(product);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.PostAsync("", content);

            ErrorResponse? response =
                await _fixture.ReadHttpResponseAsync<ErrorResponse>(httpResponse, _options);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.NotNull(response);
        }

        /// <summary>
        /// Tests that 'post' request to the 'https://api/v1/products' endpoint when the token was not sent,
        /// should returns a 401 Unauthorized status code response.
        /// </summary>
        [Fact]
        public async Task PostProduct_WhenNotAuthorized_ShouldReturn401Unauthorized()
        {
            //Arrange
            CreateProductCommandRequest product = new AutoFaker<CreateProductCommandRequest>()
            .Ignore(p => p.CategoryId)
            .Ignore(p => p.Name)
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url())
            .RuleFor(p => p.CategoryId, f => f
                .PickRandom(_fixture.DbContext.Categories
                    .Select(c => c.Id)
                    .ToList())).Generate();

            StringContent content = _fixture.CreateStringContent(product);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.PostAsync("", content);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }
        
        /// <summary>
        /// Tests that 'post' request to the 'https://api/v1/products/category-name' endpoint when product 
        /// is valid, should returns a 201 Created status code response.
        /// </summary>
        [Fact]
        public async Task PostProductByCategoryName_WhenProductIsValid_ShouldReturn201Created()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateProductByCatNameCommandRequest product = new AutoFaker<CreateProductByCatNameCommandRequest>()
            .Ignore(p => p.CategoryId)
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url())
            .RuleFor(p => p.CategoryName, f => f
                .PickRandom(_fixture.DbContext.Categories
                    .Select(c => c.Name)
                    .ToList())).Generate();

            StringContent content = _fixture.CreateStringContent(product);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.PostAsync("category-name", content);

            CreateProductCommandResponse? response =
                await _fixture.ReadHttpResponseAsync<CreateProductCommandResponse>(httpResponse, _options);

            //Assert
            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(product.Name, response.Name);
        }

        /// <summary>
        /// Tests that 'post' request to the 'https://api/v1/products/category-name' endpoint when product 
        /// is invalid, should returns a 400 Bad Request status code response.
        /// </summary>
        [Fact]
        public async Task PostProductByCategoryName_WhenProductIsInvalid_ShouldReturn400BadRequest()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            CreateProductByCatNameCommandRequest product = new AutoFaker<CreateProductByCatNameCommandRequest>()
            .Ignore(p => p.CategoryId)
            .Ignore(p => p.Name)
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url())
            .RuleFor(p => p.CategoryName, f => f
                .PickRandom(_fixture.DbContext.Categories
                    .Select(c => c.Name)
                    .ToList())).Generate();

            StringContent content = _fixture.CreateStringContent(product);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.PostAsync("category-name", content);

            ErrorResponse? response =
                await _fixture.ReadHttpResponseAsync<ErrorResponse>(httpResponse, _options);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.NotNull(response);
        }

        /// <summary>
        /// Tests that 'post' request to the 'https://api/v1/products/category-name' endpoint when the token 
        /// was not sent, should returns a 401 Unauthorized status code response.
        /// </summary>
        [Fact]
        public async Task PostProductByCategoryName_WhenNotAuthorized_ShouldReturn401Unauthorized()
        {
            //Arrange
            CreateProductByCatNameCommandRequest product = new AutoFaker<CreateProductByCatNameCommandRequest>()
            .Ignore(p => p.CategoryId)
            .Ignore(p => p.Name)
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url())
            .RuleFor(p => p.CategoryName, f => f
                .PickRandom(_fixture.DbContext.Categories
                    .Select(c => c.Name)
                    .ToList())).Generate();

            StringContent content = _fixture.CreateStringContent(product);

            //Act
            HttpResponseMessage httpResponse = await _httpClient.PostAsync("category-name", content);
            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }

        /// <summary>
        /// Tests that 'put' request to the 'https://api/v1/products/{id}' endpoint when product 
        /// is valid, should returns a 200 OK status code response.
        /// </summary>
        [Fact]
        public async Task PutProduct_WhenProductIsValid_ShouldReturn200OK()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
            (
                scheme: "Bearer",
                parameter: token
            );

            Guid id = _fixture.DbContext.Products.First().Id;
            UpdateProductCommandRequest request = new AutoFaker<UpdateProductCommandRequest>().RuleFor(p => p.ImageUrl, f => f.Internet.Url());

            StringContent content = _fixture.CreateStringContent(request);
            
            //Act
            HttpResponseMessage httpResponse = await _httpClient.PutAsync(id.ToString(), content);
            UpdateProductCommandResponse? response = await _fixture.ReadHttpResponseAsync<UpdateProductCommandResponse>(httpResponse, _options); 

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.NotNull(response);
            Assert.Equal(id, response.Id);
            Assert.Equal(request.Name, response.Name);
        }

        /// <summary>
        /// Tests that 'put' request to the 'https://api/v1/products/{id}' endpoint when product 
        /// is invalid, should returns a 400 Bad Request status code response.
        /// </summary>
        [Fact]
        public async Task PutProduct_WhenProductIsInvalid_ShouldReturn400BadRequest()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
            (
                scheme: "Bearer",
                parameter: token
            );

            Guid id = _fixture.DbContext.Products.First().Id;
            UpdateProductCommandRequest request = new AutoFaker<UpdateProductCommandRequest>()
            .Ignore(p => p.Name)
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url());

            StringContent content = _fixture.CreateStringContent(request);
            
            //Act
            HttpResponseMessage httpResponse = await _httpClient.PutAsync(id.ToString(), content);
            ErrorResponse? response = await _fixture.ReadHttpResponseAsync<ErrorResponse>(httpResponse, _options); 

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.NotNull(response);
        }
    
        /// <summary>
        /// Tests that 'put' request to the 'https://api/v1/products/{id}' endpoint when product 
        /// is valid, but product id not exists, should returns a 404 Not Found status code response.
        /// </summary>
        [Fact]
        public async Task PutProduct_WhenProductIdNotExists_ShouldReturn404NotFound()
        {
            //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
            (
                scheme: "Bearer",
                parameter: token
            );

            Guid id = Guid.NewGuid();
            UpdateProductCommandRequest request = new AutoFaker<UpdateProductCommandRequest>()
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url());

            StringContent content = _fixture.CreateStringContent(request);
            
            //Act
            HttpResponseMessage httpResponse = await _httpClient.PutAsync(id.ToString(), content);
            ErrorResponse? response = await _fixture.ReadHttpResponseAsync<ErrorResponse>(httpResponse, _options); 

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.NotNull(response);
        }

        
        /// <summary>
        /// Tests that 'put' request to the 'https://api/v1/products/{id}' endpoint when the token was not sent,
        /// should returns a 401 Unauthorized status code response.
        /// </summary>
        [Fact]
        public async Task PutProduct_WhenNotAuthorized_ShouldReturn401Unauthorized()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            UpdateProductCommandRequest request = new AutoFaker<UpdateProductCommandRequest>()
            .RuleFor(p => p.ImageUrl, f => f.Internet.Url());

            StringContent content = _fixture.CreateStringContent(request);
            
            //Act
            HttpResponseMessage httpResponse = await _httpClient.PutAsync(id.ToString(), content);

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }

        /// <summary>
        /// Tests that 'delete' request to the 'https://api/v1/products/{id}' endpoint when product id
        /// exists, should returns a 200 OK.
        /// </summary>
        [Fact]
        public async Task DeleteProduct_WhenProductIdExists_ShouldReturn200OK()
        {
               //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
            (
                scheme: "Bearer",
                parameter: token
            );

            Guid id = _fixture.DbContext.Products.First().Id;

            //Act
            HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(id.ToString());

            DeleteProductCommandResponse? response = 
                await _fixture.ReadHttpResponseAsync<DeleteProductCommandResponse>(httpResponse, _options);

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.NotNull(response);
        }

        
        /// <summary>
        /// Tests that 'delete' request to the 'https://api/v1/products/{id}' endpoint when product id
        /// not exists, should returns a 404 Not Found.
        /// </summary>
        [Fact]
        public async Task DeleteProduct_WhenProductIdNotExists_ShouldReturn404NotFound()
        {
               //Arrange
            string token = _fixture.GenerateTokenAdmin();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
            (
                scheme: "Bearer",
                parameter: token
            );

            Guid id = Guid.NewGuid();

            //Act
            HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(id.ToString());

            ErrorResponse? response = 
                await _fixture.ReadHttpResponseAsync<ErrorResponse>(httpResponse, _options);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
            Assert.NotNull(response);
        }
        
        /// <summary>
        /// Tests that 'delete' request to the 'https://api/v1/products/{id}' endpoint when the token was
        /// not sent, should returns a 401 Unauthorized.
        /// </summary>
        [Fact]
        public async Task DeleteProduct_WhenNotAuthorized_ShouldReturn401Unauthorized()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            HttpResponseMessage httpResponse = await _httpClient.DeleteAsync(id.ToString());

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }
    }
}