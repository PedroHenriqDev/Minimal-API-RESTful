using Microsoft.OpenApi.Models;

namespace Catalogue.API.Extensions;

public static class EndpointConventionBuilderExtension
{
    public static void WithLoginDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Login User",
            Description = "Authenticates a user and generates a JWT token if the login is successful.",
            Tags = new List<OpenApiTag> { new OpenApiTag() {Name = "Authentication"}}
        });
    }

    public static void WithRegisterDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Register User",
            Description = "Registers a new user.",
            Tags = new List<OpenApiTag>{new OpenApiTag() {Name = "Authentication"}}
        });      
    }

    public static void WithPutRoleDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Update user role",
            Description = "Updates the role of a user specified by their ID. This endpoint is restricted to users with the 'AdminOnly' policy.",
            Tags = new List<OpenApiTag>{new OpenApiTag() {Name = "Authentication"}}
        });    
    }

    public static void WithPutUserDoc(this IEndpointConventionBuilder builder)
    {
        builder.WithOpenApi(operation => new(operation)
        {
            Summary = "Updates user informations.",
            Description = "Updates the user. This endpoint use a custom endpoint filter, is used to inject the current authenticated user's name into the request object before it's processed.",
            Tags = new List<OpenApiTag>{new OpenApiTag() {Name = "Authentication"}}
        });    
    }
}