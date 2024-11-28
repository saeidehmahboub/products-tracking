using Backend.Data;
using Backend.Dto;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class EventLogsRepository : IEventLogsRepository
    {
        private readonly DataContext _context;
        public EventLogsRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateEventLog(EventLog eventLog)
        {
            _context.Add(eventLog);
            return Save();
        }

        public bool DeleteEventLog(EventLog eventLog)
        {
            _context.Remove(eventLog);
            return Save();
        }

        public bool EventLogExistsForProduct(int id)
        {
            return _context.EventLogs.Any(e => e.ProductId == id);
        }

        public EventLog GetEventLog(int id)
        {
            return _context.EventLogs.Where(e => e.Id == id).FirstOrDefault();
        }

        public PaginationDto<EventLogDto> GetEventLogs(int pageNumber = 1, int pageSize = 10)
        {
           
            var totalItems = _context.EventLogs.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

            var result = _context.EventLogs
                .Include(e => e.Product)
                .OrderBy(e => e.Id)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)
                .Select(e => new EventLogDto
                {
                    Id = e.Id,
                    ProductId = e.ProductId,
                    ProductName = e.Product.Name,
                    Event = e.Event.ToString(),
                    CreatedAt = e.CreatedAt
                })
                .ToList();

            return new PaginationDto<EventLogDto>
            {
                Result = result,
                TotalPages = totalPages
            };
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}