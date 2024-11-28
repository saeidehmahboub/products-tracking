using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepositopry _reportsRepositopry;
        public ReportsController(IReportsRepositopry reportsRepositopry)
        {
            _reportsRepositopry = reportsRepositopry;
        }

        [HttpGet("papular-products")]
        public IActionResult GetPopularProducts()
        {
            var papularProducts = _reportsRepositopry.GetPopularProducts();
            return Ok(papularProducts);
        }
    }
    
}