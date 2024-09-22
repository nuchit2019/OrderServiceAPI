using Microsoft.AspNetCore.Mvc;
using OrderServiceAPI.Models;
using OrderServiceAPI.Models.response;
using OrderServiceAPI.Services;
using System.Net;

namespace OrderServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(new ApiResponse<IEnumerable<Order>>(true, HttpStatusCode.OK,orders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) 
                return NotFound(new ApiResponse<string>(false, HttpStatusCode.NotFound, "Order not found"));
            return Ok(new ApiResponse<Order>(true, HttpStatusCode.OK, order));

        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderService.CreateOrderAsync(order);
            var createdOrderId = order.OrderId;
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, new ApiResponse<Order>(true, HttpStatusCode.Created, order));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (id != order.OrderId) return BadRequest();
            await _orderService.UpdateOrderAsync(order);
            return Ok(new ApiResponse<string>(true, HttpStatusCode.OK, "UpdateOrder Order success"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok(new ApiResponse<string>(true, HttpStatusCode.OK, "Delete Order success"));
        }

        [HttpGet("sum")]
        public async Task<IActionResult> SumOrder()
        {
            var sum = await _orderService.SumOrderAsync();
            return Ok(new ApiResponse<decimal>(true, HttpStatusCode.OK, sum));
        }
    }
}