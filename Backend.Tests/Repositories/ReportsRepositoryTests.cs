using Xunit;
using FluentAssertions;
using Backend.Repository;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

public class ReportsRepositoryTests
{
    private readonly ReportsRepositopry _repository;
    private readonly DataContext _context;

    public ReportsRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("TestDatabase_Reports")
            .Options;

        _context = new DataContext(options);

        _context.Products.AddRange(
            new Product { Id = 1, Name = "Laptop", Price = 1000 },
            new Product { Id = 2, Name = "Phone", Price = 500 }
        );

        _context.EventLogs.AddRange(
            new EventLog { Id = 1, ProductId = 1, Event = EventType.view, CreatedAt = DateTime.UtcNow },
            new EventLog { Id = 2, ProductId = 1, Event = EventType.view, CreatedAt = DateTime.UtcNow },
            new EventLog { Id = 3, ProductId = 2, Event = EventType.add_to_cart, CreatedAt = DateTime.UtcNow }
        );

        _context.SaveChanges();

        _repository = new ReportsRepositopry(_context);
    }

    [Fact]
    public void GetPopularProducts_ShouldReturnCorrectData()
    {
        var result = _repository.GetPopularProducts();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().ProductName.Should().Be("Laptop");
    }
}