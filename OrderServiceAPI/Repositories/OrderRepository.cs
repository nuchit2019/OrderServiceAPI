using OrderServiceAPI.Data;
using OrderServiceAPI.Models;

namespace OrderServiceAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<int> CreateOrderAsync(Order order);
        Task<int> UpdateOrderAsync(Order order);
        Task<int> DeleteOrderAsync(int id);
        Task<decimal> SumOrderAsync();
    }


    public class OrderRepository : IOrderRepository
    {
        private readonly ISqlDataAccess _db;

        public OrderRepository(ISqlDataAccess db)
        {
            // เปิดใช้งานการแมพพารามิเตอร์โดยไม่สนใจตัวใหญ่ตัวเล็ก
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _db = db;
        }

        public Task<IEnumerable<Order>> GetOrdersAsync() =>
            _db.QueryAsync<Order>("SELECT * FROM Orders");

        public async Task<Order?> GetOrderByIdAsync(int id) => await _db.QueryAsync<Order>("SELECT * FROM Orders WHERE OrderId = @id", new {id}  ).ContinueWith(x => x.Result.FirstOrDefault());

        public async Task<int> CreateOrderAsync(Order order) => await _db.ExecuteAsync("INSERT INTO Orders (OrderId,ProductName, Quantity, Price) VALUES (@OrderId, @ProductName, @Quantity, @Price)", order);

        public async Task<int> UpdateOrderAsync(Order order) => await _db.ExecuteAsync("UPDATE Orders SET ProductName = @ProductName, Quantity = @Quantity, Price = @Price WHERE OrderId = @OrderId", order);

        public async Task<int> DeleteOrderAsync(int id) => await _db.ExecuteAsync("DELETE FROM Orders WHERE OrderId = @id",   new { id } );

        public Task<decimal> SumOrderAsync() => _db.QueryAsync<decimal>("SELECT SUM(Price * Quantity) FROM Orders").ContinueWith(x => x.Result.FirstOrDefault());
    }
}
