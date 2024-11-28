using Backend.Data;
using Backend.Models;
using Backend.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests.Repositories
{
    public class ProductsRepositoryTests
    {
        private readonly ProductsRepository _repository;
        private readonly DataContext _context;

        public ProductsRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Products")
                .Options;

            _context = new DataContext(options);
            _repository = new ProductsRepository(_context);
        }

        [Fact]
        public void CreateProduct_ShouldAddProductToDatabase()
        {
            var product = new Product { Name = "Test Product", Price = 100 };

            var result = _repository.CreateProduct(product);

            result.Should().BeTrue();
            _context.Products.Should().ContainSingle(p => p.Name == "Test Product");
        }

        [Fact]
        public void GetProducts_ShouldReturnPaginatedProducts()
        {
            _context.Products.AddRange(new List<Product>
            {
                new Product { Name = "Product 1", Price = 50 },
                new Product { Name = "Product 2", Price = 60 },
                new Product { Name = "Product 3", Price = 70 }
            });
            _context.SaveChanges();

            var result = _repository.GetProducts(1, 2);

            result.Should().NotBeNull();
            result.Result.Should().HaveCount(2);
            result.TotalPages.Should().Be(3);
        }

        [Fact]
        public void ProductExists_ShouldReturnTrueIfProductExists()
        {
            var product = new Product { Name = "Existing Product", Price = 200 };
            _context.Products.Add(product);
            _context.SaveChanges();

            var result = _repository.ProductExists(product.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteProduct_ShouldRemoveProductFromDatabase()
        {
            var product = new Product { Name = "Product To Delete", Price = 80 };
            _context.Products.Add(product);
            _context.SaveChanges();

            var result = _repository.DeleteProduct(product);

            result.Should().BeTrue();
            _context.Products.Should().NotContain(p => p.Name == "Product To Delete");
        }

        [Fact]
        public void UpdateProduct_ShouldModifyExistingProduct()
        {
            var product = new Product { Name = "Old Name", Price = 90 };
            _context.Products.Add(product);
            _context.SaveChanges();

            product.Name = "Updated Name";

            var result = _repository.UpdateProduct(product);

            result.Should().BeTrue();
            _context.Products.Should().ContainSingle(p => p.Name == "Updated Name");
        }
    }
}
