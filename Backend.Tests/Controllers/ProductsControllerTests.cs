using Backend.Controllers;
using Backend.Interfaces;
using Backend.Models;
using Backend.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Backend.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductsRepository> _mockProductsRepository;
        private readonly Mock<IEventLogsRepository> _mockEventLogsRepository;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductsRepository = new Mock<IProductsRepository>();
            _mockEventLogsRepository = new Mock<IEventLogsRepository>();
            _controller = new ProductsController(_mockProductsRepository.Object, _mockEventLogsRepository.Object);
        }

        [Fact]
        public void GetProducts_ReturnsOk_WithPaginationDto()
        {
            var pageNumber = 1;
            var pageSize = 10;
            var paginationResult = new PaginationDto<Product>
            {
                Result = new List<Product>
                {
                    new Product { Id = 1, Name = "Product 1", Price = 100 },
                    new Product { Id = 2, Name = "Product 2", Price = 200 }
                },
                TotalPages = 1
            };

            _mockProductsRepository
                .Setup(repo => repo.GetProducts(pageNumber, pageSize))
                .Returns(paginationResult);

            var result = _controller.GetProducts(pageNumber, pageSize) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<PaginationDto<Product>>(result.Value);
        }

        [Fact]
        public void GetProduct_ReturnsOk_WhenProductExists()
        {
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product 1", Price = 100 };

            _mockProductsRepository
                .Setup(repo => repo.ProductExists(productId))
                .Returns(true);

            _mockProductsRepository
                .Setup(repo => repo.GetProduct(productId))
                .Returns(product);

            var result = _controller.GetProduct(productId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<Product>(result.Value);
            Assert.Equal(product, result.Value);
        }

        [Fact]
        public void GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var productId = 1;

            _mockProductsRepository
                .Setup(repo => repo.ProductExists(productId))
                .Returns(false);

            var result = _controller.GetProduct(productId) as NotFoundResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void CreateProduct_ReturnsOk_WhenSuccessful()
        {
            var product = new Product { Id = 1, Name = "Product 1", Price = 100 };

            _mockProductsRepository
                .Setup(repo => repo.CreateProduct(product))
                .Returns(true);

            var result = _controller.CreateProduct(product) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True((bool)result.Value);
        }

        [Fact]
        public void CreateProduct_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var product = new Product { Id = 1, Price = 100 };

            var result = _controller.CreateProduct(product) as BadRequestResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void DeleteProduct_ReturnsNoContent_WhenSuccessful()
        {
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product 1", Price = 100 };

            _mockProductsRepository
                .Setup(repo => repo.ProductExists(productId))
                .Returns(true);

            _mockProductsRepository
                .Setup(repo => repo.GetProduct(productId))
                .Returns(product);

            _mockEventLogsRepository
                .Setup(repo => repo.EventLogExistsForProduct(productId))
                .Returns(false);

            _mockProductsRepository
                .Setup(repo => repo.DeleteProduct(product))
                .Returns(true);

            var result = _controller.DeleteProduct(productId) as NoContentResult;

            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public void DeleteProduct_ReturnsBadRequest_WhenProductIsReferencedInEventLog()
        {
            var productId = 1;
            var product = new Product { Id = productId, Name = "Product 1", Price = 100 };

            _mockProductsRepository
                .Setup(repo => repo.ProductExists(productId))
                .Returns(true);

            _mockProductsRepository
                .Setup(repo => repo.GetProduct(productId))
                .Returns(product);

            _mockEventLogsRepository
                .Setup(repo => repo.EventLogExistsForProduct(productId))
                .Returns(true);

            var result = _controller.DeleteProduct(productId) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Cannot delete the product because it is referenced in EventLog.", result.Value);

            _mockEventLogsRepository
                .Verify(repo => repo.EventLogExistsForProduct(productId), Times.Once);
        }


    }
}
