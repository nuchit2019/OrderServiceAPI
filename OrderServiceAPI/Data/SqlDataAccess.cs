using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace OrderServiceAPI.Data
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<int> ExecuteAsync(string sql, object parameters = null);
    }

    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.QueryAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return await db.ExecuteAsync(sql, parameters);
        }
    }
}
