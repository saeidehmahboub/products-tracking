using Backend.Data;
using Backend.Dto;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class ReportsRepositopry : IReportsRepositopry
    {
        private readonly DataContext _context;
        public ReportsRepositopry(DataContext context)
        {
            _context = context;
        }
        
        public ICollection<ProductEventCountDto> GetPopularProducts()
        {
            var result = _context.EventLogs
            .GroupBy(e => e.ProductId)
            .Select(g => new ProductEventCountDto
            {
                ProductId = g.Key,
                CountEvent = g.Count(),
                ProductName = g.First().Product.Name
            })
            .OrderByDescending(x => x.CountEvent)
            .Take(10)
            .ToList();

            return result;

        }
    }
}
