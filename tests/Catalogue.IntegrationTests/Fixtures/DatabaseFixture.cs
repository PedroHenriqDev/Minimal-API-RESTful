using AutoBogus;
using Catalogue.Domain.Entities;
using Catalogue.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.IntegrationTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public AppDbContext DbContext { get; set; }

    /// <summary>
    ///  Initializes an instance of the 'AppDbContext' with sample data for testing.
    /// </summary>
    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        DbContext = new AppDbContext(options);

        var userAutoFaker = new AutoFaker<User>().RuleFor(u => u.Email, f => f.Internet.Email());
        var categoryAutoFaker = new AutoFaker<Category>();
        var productAutoFaker = new AutoFaker<Product>();

        DbContext.Users.AddRange(userAutoFaker.Generate(10));
        DbContext.Categories.AddRange(categoryAutoFaker.Generate(10));
        DbContext.Products.AddRange(productAutoFaker.Generate(10));

        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}
