using Catalogue.API.Endpoints;
using Catalogue.API.Filters;
using Catalogue.CrossCutting.AppDependencies;
using Catalogue.Domain.Enums;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { builder.Configuration["Env:Path"], ".env" }));

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GlobalExceptionFilter>();
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Categories Endpoints
app.MapGetCategoriesEndpoints();
app.MapPostCategoriesEndpoints();
app.MapDeleteCategoriesEndpoints();
app.MapPutCategoriesEndpoints();

//Products Endpoints
app.MapGetProductsEndpoints();
app.MapPostProductsEndpoints();
app.MapDeleteProductsEndpoints();
app.MapPutProductsEndpoints();

//Users Endpoints
app.MapPostAuthEndpoints();
app.MapPutAuthEndpoints();

app.UseCors(builder.Configuration["Cors:PolicyName"]!);

app.UseAuthentication();
app.UseAuthorization();

app.UseGlobalExceptionFilter();
app.UseHttpsRedirection();

app.Run();
