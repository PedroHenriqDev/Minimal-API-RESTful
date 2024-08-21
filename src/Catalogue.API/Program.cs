using Catalogue.API.Endpoints;
using Catalogue.API.Extensions;
using Catalogue.API.Filters;
using Catalogue.CrossCutting.AppDependencies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GlobalExceptionFilter>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPostCategoryEndpoints();
app.MapDeleteCategoryEndpoints();
app.MapUpdateCategoryEndpoints();

app.UseGlobalExceptionFilter();
app.UseHttpsRedirection();

app.Run();



string email = string.Empty;

bool IsEmail(string email) 
{
    return email.Contains("@");
}