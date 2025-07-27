using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TaskManagement.Models;
using Dapper;

namespace TaskManagement.Repositories.TaskRepository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT * FROM Tasks";
                return await conn.QueryAsync<TaskItem>(sql);
            }
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT * FROM Tasks WHERE Id = @Id";
                return await conn.QuerySingleOrDefaultAsync<TaskItem>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(string status)
        {
            using (var conn = CreateConnection())
            {
                string sql = "SELECT * FROM Tasks WHERE Status = @Status";
                return await conn.QueryAsync<TaskItem>(sql, new { Status = status });
            }
        }

        public async Task CreateAsync(TaskItem entity)
        {
            using (var conn = CreateConnection())
            {
                string sql = @"
                INSERT INTO Tasks (Title, Description, Status, CreatedAt)
                VALUES (@Title, @Description, @Status, @CreatedAt)";
                await conn.ExecuteAsync(sql, entity);
            }
        }

        public async Task UpdateAsync(TaskItem entity)
        {
            using (var conn = CreateConnection())
            {
                string sql = @"
                UPDATE Tasks
                SET Title = @Title,
                    Description = @Description,
                    Status = @Status
                WHERE Id = @Id";
                await conn.ExecuteAsync(sql, entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var conn = CreateConnection())
            {
                string sql = "DELETE FROM Tasks WHERE Id = @Id";
                await conn.ExecuteAsync(sql, new { Id = id });
            }
        }
    }

}