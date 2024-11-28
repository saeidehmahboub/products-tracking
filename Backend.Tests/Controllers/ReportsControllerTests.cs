using Backend.Controllers;
using Backend.Interfaces;
using Backend.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace Backend.Tests.Controllers
{
    public class ReportsControllerTests
    {
        private readonly Mock<IReportsRepositopry> _mockReportsRepository;
        private readonly ReportsController _controller;

        public ReportsControllerTests()
        {
            _mockReportsRepository = new Mock<IReportsRepositopry>();
            _controller = new ReportsController(_mockReportsRepository.Object);
        }

        [Fact]
        public void GetPopularProducts_ReturnsOkResult_WithData()
        {
            // Arrange
            var popularProducts = new List<ProductEventCountDto>
            {
                new ProductEventCountDto { ProductId = 1, ProductName = "Product 1", CountEvent = 10 },
                new ProductEventCountDto { ProductId = 2, ProductName = "Product 2", CountEvent = 7 }
            };

            _mockReportsRepository
                .Setup(repo => repo.GetPopularProducts())
                .Returns(popularProducts);

            // Act
            var result = _controller.GetPopularProducts() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var returnedProducts = result.Value as IEnumerable<ProductEventCountDto>;
            Assert.NotNull(returnedProducts);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public void GetPopularProducts_ReturnsOkResult_WithEmptyList_WhenNoDataExists()
        {
            // Arrange
            _mockReportsRepository
                .Setup(repo => repo.GetPopularProducts())
                .Returns(new List<ProductEventCountDto>());

            // Act
            var result = _controller.GetPopularProducts() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var returnedProducts = result.Value as IEnumerable<ProductEventCountDto>;
            Assert.NotNull(returnedProducts);
            Assert.Empty(returnedProducts);
        }
    }
}
