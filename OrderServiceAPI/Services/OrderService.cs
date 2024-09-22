using OrderServiceAPI.Models;
using OrderServiceAPI.Repositories;

namespace OrderServiceAPI.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<int> CreateOrderAsync(Order order);
        Task<int> UpdateOrderAsync(Order order);
        Task<int> DeleteOrderAsync(int id);
        Task<decimal> SumOrderAsync();
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        { 
            _orderRepository = orderRepository;
        }

        public Task<IEnumerable<Order>> GetOrdersAsync() => _orderRepository.GetOrdersAsync();

        public Task<Order> GetOrderByIdAsync(int id) => _orderRepository.GetOrderByIdAsync(id);

        public Task<int> CreateOrderAsync(Order order) => _orderRepository.CreateOrderAsync(order);

        public Task<int> UpdateOrderAsync(Order order) => _orderRepository.UpdateOrderAsync(order);

        public Task<int> DeleteOrderAsync(int id) => _orderRepository.DeleteOrderAsync(id);

        public Task<decimal> SumOrderAsync() => _orderRepository.SumOrderAsync();
    }
}
