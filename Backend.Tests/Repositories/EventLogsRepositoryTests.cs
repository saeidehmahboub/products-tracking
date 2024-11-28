using Backend.Repository;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using FluentAssertions;

public class EventLogsRepositoryTests
{
    private readonly EventLogsRepository _repository;
    private readonly DataContext _context;

    public EventLogsRepositoryTests()
    {
        
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("TestDatabase_EventLogs")
            .Options;

        _context = new DataContext(options);


        _context.Products.Add(new Product { Id = 1, Name = "Laptop", Price = 1000 });
        _context.EventLogs.Add(new EventLog { Id = 1, ProductId = 1, Event = EventType.view, CreatedAt = DateTime.UtcNow });
        _context.SaveChanges();

        _repository = new EventLogsRepository(_context);
    }

    [Fact]
    public void GetEventLogs_ShouldReturnData()
    {
        var result = _repository.GetEventLogs();

        result.Should().NotBeNull();
        result.Result.Should().HaveCount(1);
    }
}
