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
    public class AcquisitionCategoryRepository : IAcquisitionCategoryRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionCategoryRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> DeleteCategory(AcquisitionCategory acquisitionCategory)
        {
            var db = dbConnection();

            var sql = @" UPDATE acquisitioncategories
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { acquisitionCategory.Status, acquisitionCategory.Id });

            return result > 0;
        }

        public async Task<IEnumerable<AcquisitionCategory>> GetAllCategories()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Status
                        FROM acquisitioncategories";

            return await db.QueryAsync<AcquisitionCategory>(sql, new { });
        }

        public async Task<AcquisitionCategory> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Status
                        FROM acquisitioncategories
                        WHERE acquisitioncategories.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionCategory>(sql, new { Id = id });
        }

        public async Task<bool> InsertCategory(AcquisitionCategory acquisitionCategory)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitioncategories(Name)
                        VALUES (@Name)";

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionCategory.Name
            });

            return result > 0;
        }

        public async Task<bool> UpdateCategory(AcquisitionCategory acquisitionCategory)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitioncategories SET ");

            if (!string.IsNullOrWhiteSpace(acquisitionCategory.Name))
                sqlBuilder.Append("Name = @Name,");

            if (acquisitionCategory.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionCategory.Name,
                acquisitionCategory.Status,
                acquisitionCategory.Id
            });

            return result > 0;
        }
    }
}
