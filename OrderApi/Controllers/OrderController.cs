using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Model;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private static ConcurrentBag<Order> orderQueue = new ConcurrentBag<Order>();
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            try
            {
                if(order is null)
                {
                    return BadRequest("order is not valid");
                }
                orderQueue.Add(order);
                _logger.LogInformation("Order placed successfully: {OrderId}", order.OrderId);

                return Ok("Order placed successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the order.");
                return StatusCode(500, "An error occurred while processing the order.");
            }
        }

        [HttpGet]
        public IActionResult GetOrders(int page = 1, int pageSize = 10)
        {
            try
            {
                var orders = orderQueue.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return Ok(orders);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving orders.");
                return StatusCode(500, "An error occurred while retrieving orders.");
            }
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrder(int orderId)
        {
            try
            {
                if(orderId < 0)
                {
                    _logger.LogError("order id: {id}  is not valid", orderId);
                    BadRequest("order id is not valid");
                }
                var order = orderQueue.FirstOrDefault(o => o.OrderId == orderId);
                if(order == null)
                {
                    _logger.LogWarning("Order not found with order-id: {orderId}", orderId);
                    return NotFound("Order not found");
                }

                return Ok(order);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }

}
