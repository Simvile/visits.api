using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using visits.api.Data;

namespace visits.tests.Common.Fixtures;

public class DatabaseFixture
{
    public AppDbContext Context { get; }
    public Mock<AppDbContext> ContextWithSets { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w =>
                w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        Context = new AppDbContext(options);
    }

    public Mock<AppDbContext> CreateDbContext<T>( IQueryable<T> data, Expression<Func<AppDbContext, DbSet<T>>> dbSetExpression) where T : class
    {
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        // 👇 Provide DbContextOptions
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var mockContext = new Mock<AppDbContext>(options);

        mockContext.Setup(dbSetExpression).Returns(mockSet.Object);

        return mockContext;
    }
}