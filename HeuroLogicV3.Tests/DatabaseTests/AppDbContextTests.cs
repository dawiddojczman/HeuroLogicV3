using HeuroLogicV3.Database.Contexts;
using HeuroLogicV3.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace HeuroLogicV3.Tests.DatabaseTests;

[TestFixture]
public class AppDbContextTests
{
    [Test]
    public async Task CanAddAndDeleteDecision_InMemorySmokeTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);

        var user = new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "test-user"
        };

        db.Users.Add(user);

        var decision = new Decision
        {
            Id = Guid.NewGuid(),
            Title = "Test decision",
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id
        };

        db.Decisions.Add(decision);

        await db.SaveChangesAsync();

        Assert.That(await db.Decisions.CountAsync(), Is.EqualTo(1));

        db.Decisions.Remove(decision);

        await db.SaveChangesAsync();

        Assert.That(await db.Decisions.CountAsync(), Is.Zero);
    }
}