using Backend.Models;
using Backend.Dto;

namespace Backend.Interfaces
{
    public interface IProductsRepository
    {
        PaginationDto<Product> GetProducts(int pageNumber, int pageSize);

        Product GetProduct(int id);

        bool ProductExists(int id);

        bool CreateProduct(Product product);

        bool UpdateProduct(Product product);

        bool DeleteProduct(Product product);

        bool Save();
    }
}
