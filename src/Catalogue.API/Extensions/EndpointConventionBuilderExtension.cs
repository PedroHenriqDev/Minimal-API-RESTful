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
}