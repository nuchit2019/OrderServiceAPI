using Moq;
using OrderServiceAPI.Models;
using OrderServiceAPI.Repositories;
using OrderServiceAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestOrderServiceAPI.ServicesTest
{
    public class Test_OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly OrderService _orderService;

        public Test_OrderServiceTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _orderService = new OrderService(_mockOrderRepository.Object);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsOrders()
        {
            // Arrange
            var orders = new List<Order> { new Order { OrderId = 1 }, new Order { OrderId = 2 } };
            _mockOrderRepository.Setup(repo => repo.GetOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrder_WhenExists()
        {
            // Arrange
            var order = new Order { OrderId = 1 };
            _mockOrderRepository.Setup(repo => repo.GetOrderByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(order.OrderId, result.OrderId);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            _mockOrderRepository.Setup(repo => repo.GetOrderByIdAsync(1)).ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.GetOrderByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateOrderAsync_ReturnsOrderId()
        {
            // Arrange
            var order = new Order { OrderId = 1 };
            _mockOrderRepository.Setup(repo => repo.CreateOrderAsync(order)).ReturnsAsync(order.OrderId);

            // Act
            var result = await _orderService.CreateOrderAsync(order);

            // Assert
            Assert.Equal(order.OrderId, result);
        }

        [Fact]
        public async Task UpdateOrderAsync_ReturnsUpdatedOrderId()
        {
            // Arrange
            var order = new Order { OrderId = 1 };
            _mockOrderRepository.Setup(repo => repo.UpdateOrderAsync(order)).ReturnsAsync(order.OrderId);

            // Act
            var result = await _orderService.UpdateOrderAsync(order);

            // Assert
            Assert.Equal(order.OrderId, result);
        }

        [Fact]
        public async Task DeleteOrderAsync_ReturnsDeletedOrderId()
        {
            // Arrange
            var orderId = 1;
            _mockOrderRepository.Setup(repo => repo.DeleteOrderAsync(orderId)).ReturnsAsync(orderId);

            // Act
            var result = await _orderService.DeleteOrderAsync(orderId);

            // Assert
            Assert.Equal(orderId, result);
        }

        [Fact]
        public async Task SumOrderAsync_ReturnsSumOfOrders()
        {
            // Arrange
            var sum = 100m;
            _mockOrderRepository.Setup(repo => repo.SumOrderAsync()).ReturnsAsync(sum);

            // Act
            var result = await _orderService.SumOrderAsync();

            // Assert
            Assert.Equal(sum, result);
        }
    }
}
