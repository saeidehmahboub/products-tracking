using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Interfaces;
using Backend.Dto;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventLogsController : ControllerBase
    {
        
        private readonly IEventLogsRepository _eventLogsRepository;
        private readonly IProductsRepository _productsRepository;
        public EventLogsController(IEventLogsRepository eventLogsRepository, IProductsRepository productsRepository)
        {
            _eventLogsRepository = eventLogsRepository;
            _productsRepository = productsRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EventLogDto>))]
        public IActionResult GetEventLogs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var eventLogs = _eventLogsRepository.GetEventLogs(pageNumber, pageSize);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(eventLogs);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] EventLogCreateDto eventLogDto)
        {
            if (eventLogDto == null) {
                return BadRequest(ModelState);
            }

            if (eventLogDto.ProductId <= 0)
            {
                return BadRequest("Invalid ProductId");
            }

            if (!_productsRepository.ProductExists(eventLogDto.ProductId))
            {
                return BadRequest("Product does not exist.");
            }

            var eventLog = new EventLog
            {
                ProductId = eventLogDto.ProductId,
                Event = (EventType)eventLogDto.Event,
                CreatedAt = DateTime.UtcNow
            };

            var result = _eventLogsRepository.CreateEventLog(eventLog);

            if (!result)
            {
                return StatusCode(500, "Failed to create the event log.");
            }

            return Ok(result);
        }


        [HttpDelete("{eventLogId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteEventLog(int eventLogId)
        {   

            var eventLogToDelete = _eventLogsRepository.GetEventLog(eventLogId);

            if (eventLogToDelete == null)
            {
                return NotFound();
            }

            if (!_eventLogsRepository.DeleteEventLog(eventLogToDelete))
            {
                return BadRequest("Failed to delete the product.");
            }

            return NoContent();
        }

    }
}