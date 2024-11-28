using Backend.Data;
using Backend.Interfaces;
using Backend.Models;
using Backend.Dto;

namespace Backend.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly DataContext _context;
        public ProductsRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateProduct(Product product)
        {
            _context.Add(product);

            return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Where(p => p.Id == id).FirstOrDefault();
        }

        public PaginationDto<Product> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = _context.Products.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            var result = _context.Products
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginationDto<Product>{ Result = result, TotalPages = totalPages};
            
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProduct(Product product)
        {
            _context.Update(product);
            return Save();
        }
    }
}