using System.Security.Cryptography.X509Certificates;
using Microsoft.OpenApi.Models;

namespace Catalogue.API.OpenApi;

public static class EndpointConventionBuilderExtension
{
    private static string authTag = "Authentication";
    private static string categoriesTag = "Categories"; 
    private const string productsTag = "Products";

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

            Description = @"Updates the role of a user specified by their ID. This endpoint is restricted
            to users with the 'AdminOnly' policy.",

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
            Summary = "Retrieves a category by its unique identifier (GUID)",

            Description = @"This endpoint uses a `GUID` as a route parameter to
                            identify the category.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }

    public static void WithGetCategoriesWithProductsDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Get categories with their associated products",

            Description = @"This endpoint Returns a paginated list of
                            categories with their associated products, along with pagination
                            metadata in the response header.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }
    
    public static void WithGetCategoryIncludingProductsDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Get a category and its associated products based on the category ID",
            Description = "Get from the unique identifier ``Guid` of the category.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }

    public static void WithPostCategoryDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Create a new category",

            Description = @"Creates a new category along based on 
                            the request body data.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }

    public static void WithPostCategoryWithProductsDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Create a new category along with associated products",

            Description = @"This endpoint allows the creation of a new category and its associated products. 
                            provide a request body containing the category details, including its name  
                            and a list of products.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){ Name = categoriesTag}}
        });
    }

    public static void WithPutCategoryDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary =  "Updates a category by its unique identifier",

            Description = @"This endpoint allows for the updating of an existing category
                            in the system by providing its unique identifier `GUID`.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }
    
    public static void WithDeleteCategoryDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Deletes a category",

            Description = "Deletes a category using its unique identifier `GUID`.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = categoriesTag}}
        });
    }

    public static void WithGetProductsDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Get products Paged",

            Description = "Get a list of products with pagination",
            
            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = productsTag}}
        });
    }

    public static void WithGetByIdProductDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Get product by id",

            Description = "Retrieves a product by its unique identifier `GUID`.",

            Tags = new List<OpenApiTag>(){new OpenApiTag(){Name = productsTag}}
        });
    }
}