using Moq;
using FluentAssertions;
using Backend.Controllers;
using Backend.Interfaces;
using Backend.Models;
using Backend.Dto;
using Microsoft.AspNetCore.Mvc;

public class EventLogsControllerTests
{
    private readonly Mock<IEventLogsRepository> _mockEventLogsRepo;
    private readonly Mock<IProductsRepository> _mockProductsRepo;
    private readonly EventLogsController _controller;

    public EventLogsControllerTests()
    {
        _mockEventLogsRepo = new Mock<IEventLogsRepository>();
        _mockProductsRepo = new Mock<IProductsRepository>();
        _controller = new EventLogsController(_mockEventLogsRepo.Object, _mockProductsRepo.Object);
    }

    [Fact]
    public void GetEventLogs_ShouldReturnOk_WhenEventLogsExist()
    {
        // Arrange
        var mockEventLogs = new PaginationDto<EventLogDto>
        {
            Result = new List<EventLogDto>
            {
                new EventLogDto { Id = 1, ProductId = 1, ProductName = "Product1", Event = "ListView", CreatedAt = DateTime.UtcNow },
                new EventLogDto { Id = 2, ProductId = 2, ProductName = "Product2", Event = "Checkout", CreatedAt = DateTime.UtcNow }
            },
            TotalPages = 1
        };

        _mockEventLogsRepo.Setup(repo => repo.GetEventLogs(1, 10)).Returns(mockEventLogs);

        // Act
        var result = _controller.GetEventLogs(1, 10);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var data = okResult.Value as PaginationDto<EventLogDto>;
        data.Should().NotBeNull();
        data!.Result.Should().HaveCount(2);
    }

    [Fact]
    public void CreateProduct_ShouldReturnBadRequest_WhenEventLogDtoIsNull()
    {
        // Act
        var result = _controller.CreateProduct(null);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
    }

    [Fact]
    public void CreateProduct_ShouldReturnBadRequest_WhenProductDoesNotExist()
    {
        // Arrange
        var eventLogDto = new EventLogCreateDto
        {
            ProductId = 99, // Invalid ProductId
            Event = 0
        };

        _mockProductsRepo.Setup(repo => repo.ProductExists(eventLogDto.ProductId)).Returns(false);

        // Act
        var result = _controller.CreateProduct(eventLogDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be("Product does not exist.");
    }

    [Fact]
    public void CreateProduct_ShouldReturnOk_WhenEventLogIsCreated()
    {
        // Arrange
        var eventLogDto = new EventLogCreateDto
        {
            ProductId = 1,
            Event = 1
        };

        _mockProductsRepo.Setup(repo => repo.ProductExists(eventLogDto.ProductId)).Returns(true);
        _mockEventLogsRepo.Setup(repo => repo.CreateEventLog(It.IsAny<EventLog>())).Returns(true);

        // Act
        var result = _controller.CreateProduct(eventLogDto);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(true);
    }

    [Fact]
    public void DeleteEventLog_ShouldReturnNotFound_WhenEventLogDoesNotExist()
    {
        // Arrange
        _mockEventLogsRepo.Setup(repo => repo.GetEventLog(99)).Returns((EventLog)null);

        // Act
        var result = _controller.DeleteEventLog(99);

        // Assert
        var notFoundResult = result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public void DeleteEventLog_ShouldReturnNoContent_WhenEventLogIsDeleted()
    {
        // Arrange
        var eventLog = new EventLog { Id = 1 };

        _mockEventLogsRepo.Setup(repo => repo.GetEventLog(1)).Returns(eventLog);
        _mockEventLogsRepo.Setup(repo => repo.DeleteEventLog(eventLog)).Returns(true);

        // Act
        var result = _controller.DeleteEventLog(1);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }
}
