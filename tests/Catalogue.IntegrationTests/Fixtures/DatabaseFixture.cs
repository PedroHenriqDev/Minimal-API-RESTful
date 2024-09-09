using AutoBogus;
using Catalogue.Domain.Entities;
using Catalogue.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.IntegrationTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public AppDbContext DbContext { get; set; }
    public List<User> Users { get; } 

    /// <summary>
    ///  Initializes an instance of the 'AppDbContext' with sample data for testing.
    /// </summary>
    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        DbContext = new AppDbContext(options);


        var autoFaker = new AutoFaker<User>().RuleFor(u => u.Email, f => f.Internet.Email());

        Users = new List<User>();
        Users.AddRange(autoFaker.Generate(10));
        DbContext.Users.AddRange(Users);
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}
