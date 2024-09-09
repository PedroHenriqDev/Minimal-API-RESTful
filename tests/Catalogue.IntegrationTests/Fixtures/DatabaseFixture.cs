using AutoBogus;
using Catalogue.Domain.Entities;
using Catalogue.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalogue.IntegrationTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public AppDbContext DbContext { get; set; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        DbContext = new AppDbContext(options);


        var autoFaker = new AutoFaker<User>().RuleFor(u => u.Email, f => f.Internet.Email());

        DbContext.Users.AddRange(autoFaker.Generate(10));
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}
