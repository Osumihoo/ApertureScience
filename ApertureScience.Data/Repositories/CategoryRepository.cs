using ApertureScience.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public CategoryRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteCategory(Category category)
        {
            var db = dbConnection();

            var sql = @" UPDATE categorys
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { category.Status, category.Id });

            return result > 0;
        }

        public async Task<IEnumerable<Category>> GetAllCategorys()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Name, Status
                        FROM categorys";

            return await db.QueryAsync<Category>(sql, new { });
        }

        public async Task<Category> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Name, Status
                        FROM categorys
                        WHERE Id = @Id";

            return await db.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id});
        }

        public async Task<bool> InsertCategory(Category category)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO categorys(Name)
                        VALUES (@Name)";

            var result = await db.ExecuteAsync(sql, new 
                { category.Name });

            return result > 0;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE categorys SET ");

            if (!string.IsNullOrWhiteSpace(category.Name))
                sqlBuilder.Append("Name = @Name,");

            if (category.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
            {
                sqlBuilder.Length--; // Elimina la última coma
            }

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            { category.Name, category.Status, category.Id });

            return result > 0;
        }
    }
}
