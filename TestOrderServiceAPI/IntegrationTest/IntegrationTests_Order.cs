using Microsoft.AspNetCore.Mvc.Testing;
using OrderServiceAPI.Models.response;
using OrderServiceAPI.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace TestOrderServiceAPI.IntegrationTest
{
    public class IntegrationTests_Order : IClassFixture<WebApplicationFactory<Program>>,IDisposable
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly JsonSerializerOptions _jsonOptions;
        private int? _createdOrderId;
        public IntegrationTests_Order(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [Fact]
        public async Task GetOrders_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/order");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<Order>>>(responseString, _jsonOptions);
            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            int orderId = 1;

            // Act
            var response = await _client.GetAsync($"/api/order/{orderId}");

            // Assert
            if (response.StatusCode == HttpStatusCode.OK)
            {
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<Order>>(responseString, _jsonOptions);
                Assert.NotNull(apiResponse);
                Assert.True(apiResponse.Success);
            }
            else
            {
                
                var responseString = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(responseString, _jsonOptions);
                Assert.NotNull(apiResponse);
                Assert.Equal("Order not found", apiResponse.Data.ToString());
                Assert.False(apiResponse.Success);
            }
        }

        [Fact]
        public async Task GetOrderById_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            int orderId = 999; 

            // Act
            var response = await _client.GetAsync($"/api/order/{orderId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtAction()
        {
            // Arrange
            var newOrder = new Order
            {
                OrderId = 99999,
                ProductName = "Product ABC",
                Quantity = 12,
                Price = 1112
            }; // Fill with valid data

            var content = new StringContent(JsonSerializer.Serialize(newOrder), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/order", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            // Deserialize the response content to get the created order
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Order>>(responseString, _jsonOptions);

            // Extract the order ID
            _createdOrderId = apiResponse?.Data?.OrderId;

            // Additional assertions
            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.NotNull(_createdOrderId);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var updatedOrder = new Order
            {
                OrderId = 4,
                ProductName = "Product AA",
                Quantity = 11,
                Price = 1111
            }; 

            var content = new StringContent(JsonSerializer.Serialize(updatedOrder), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/order/{updatedOrder.OrderId}", content);

            // Assert
            response.EnsureSuccessStatusCode();  
        }

        [Fact]
        public async Task DeleteOrder_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            int orderId = 1;

            // Act
            var response = await _client.DeleteAsync($"/api/order/{orderId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task SumOrder_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/order/sum");

            // Assert
            response.EnsureSuccessStatusCode(); 
            var responseString = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<decimal>>(responseString, _jsonOptions);
            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
             
        }

        public void Dispose()
        {
            if (_createdOrderId.HasValue)
            {
                // Cleanup: Delete the created order
                _client.DeleteAsync($"/api/order/{_createdOrderId}").Wait();
            }
        }
    }
}
