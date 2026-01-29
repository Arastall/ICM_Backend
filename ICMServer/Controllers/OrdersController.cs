using ICMServer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICMServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("ReprocessOrders")]
        public async Task<IActionResult> ReprocessOrders([FromBody] List<string> orderNumbers)
        {
            if (orderNumbers == null || orderNumbers.Count == 0)
                return BadRequest("No order numbers provided.");

            try
            {
                var results = await _orderService.ReprocessOrdersAsync(orderNumbers);
                return Ok(results);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
