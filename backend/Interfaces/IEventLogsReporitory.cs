using Backend.Dto;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IEventLogsRepository
    {
        PaginationDto<EventLogDto> GetEventLogs(int pageNumber, int pageSize);

        EventLog GetEventLog(int id);

        bool CreateEventLog(EventLog eventLog);

        bool DeleteEventLog(EventLog eventLog);

        bool EventLogExistsForProduct(int id);

        bool Save();
    }
}