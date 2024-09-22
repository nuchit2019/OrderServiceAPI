using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderServiceAPI.Controllers;
using OrderServiceAPI.Models;
using OrderServiceAPI.Models.response;
using OrderServiceAPI.Services;
using System.Net;

namespace TestOrderServiceAPI.ControllersTest
{

    public class Tests_OrderController
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController _controller;
        public Tests_OrderController()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrderController(_mockOrderService.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsOkResult_WithOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order { OrderId = 1 }, new Order { OrderId = 2 } };
            _mockOrderService.Setup(s => s.GetOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<Order>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(orders, response.Data);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOkResult_WithOrder()
        {
            // Arrange
            var order = new Order { OrderId = 1 };
            _mockOrderService.Setup(s => s.GetOrderByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrderById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Order>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(order, response.Data);
        }

        [Fact]
        public async Task GetOrderById_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderService.Setup(s => s.GetOrderByIdAsync(1)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetOrderById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Order not found", response.Data);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var order = new Order { OrderId = 1 };
            _mockOrderService.Setup(s => s.CreateOrderAsync(order)).ReturnsAsync(order.OrderId);

            // Act
            var result = await _controller.CreateOrder(order);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<Order>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(order, response.Data);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var order = new Order { OrderId = 1 };
            _mockOrderService.Setup(s => s.UpdateOrderAsync(order)).ReturnsAsync(1); 

            // Act
            var result = await _controller.UpdateOrder(1, order);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("UpdateOrder Order success", response.Data);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange           
            _mockOrderService.Setup(s => s.DeleteOrderAsync(1)).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteOrder(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Delete Order success", response.Data);
        }

        [Fact]
        public async Task SumOrder_ReturnsOkResult_WithSum()
        {
            // Arrange
            var sum = 100m;
            _mockOrderService.Setup(s => s.SumOrderAsync()).ReturnsAsync(sum);

            // Act
            var result = await _controller.SumOrder();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<decimal>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sum, response.Data);
        }
    }
}
