using Backend.Dto;

namespace Backend.Interfaces
{
    public interface IReportsRepositopry
    {
        ICollection<ProductEventCountDto> GetPopularProducts();
    }
}