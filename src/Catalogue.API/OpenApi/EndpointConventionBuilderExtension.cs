using Microsoft.OpenApi.Models;

namespace Catalogue.API.OpenApi;

public static class EndpointConventionBuilderExtension
{
    private static string authTag = "Authentication";
    private static string categoriesTag = "Categories"; 

    public static void WithLoginDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Login User",
            Description = "Authenticates a user and generates a JWT token if the login is successful.",
            Tags = new List<OpenApiTag> { new OpenApiTag() {Name = authTag}}
        });
    }

    public static void WithRegisterDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Register User",
            Description = "Registers a new user.",
            Tags = new List<OpenApiTag>{new OpenApiTag() {Name = authTag}}
        });      
    }

    public static void WithPutRoleDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Update user role",

            Description = "Updates the role of a user specified by their ID. This endpoint is restricted to users with the 'AdminOnly' policy.",

            Tags = new List<OpenApiTag>{new OpenApiTag() {Name = authTag}}
        });    
    }

    public static void WithPutUserDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Updates user informations",

            Description = @"Updates the user. This endpoint use a custom endpoint filter,
            is used to inject the current authenticated user's name into the request object
            before it's processed.",

            Tags = new List<OpenApiTag>{new OpenApiTag() {Name = authTag}}
        });    
    }

    public static void WithGetCategoriesDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Get categories paged",

            Description = @"The endpoint to retrieve a list of categories with pagination and,
            The pagination metadata includes details such as page size, current page, and total
            item count.",

            Tags = new List<OpenApiTag>{new OpenApiTag(){Name = categoriesTag}}
        });
    }

    public static void WithGetCategoryByIdDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Retrieves a category by its unique identifier (GUID).",

            Description = @"This endpoint uses a `GUID` as a route parameter to
                            identify the category.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }

    public static void WithGetCategoriesWithProductsDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Get categories with their associated products.",

            Description = @"This endpoint Returns a paginated list of
                            categories with their associated products, along with pagination
                            metadata in the response header.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }
    
    /// <summary>
    /// Retrieves a category and its associated products based on the category ID.
    /// </summary>
    /// <param name="id">The unique identifier (Guid) of the category.</param>
    /// <param name="mediator">The MediatR instance to handle the query request.</param>
    /// <returns>
    /// Returns the category along with its products if found.
    /// If the category or products are not found, it returns a 404 Not Found response.
    /// </returns>
    /// <response code="200">Category and associated products successfully retrieved.</response>
    public static void WithGetCategoryIncludingProductsDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Get a category and its associated products based on the category ID.",
            Description = "Get from the unique identifier (Guid) of the category.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }

    public static void WithPostCategoryDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Create a new category.",

            Description = @"Creates a new category along based on 
                            the request body data.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }
}