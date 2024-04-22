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
    public class AcquisitionProductRepository : IAcquisitionProductRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionProductRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteProduct(AcquisitionProduct acquisitionProduct)
        {
            var db = dbConnection();

            var sql = @" UPDATE acquisitionproducts
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { acquisitionProduct.Status, acquisitionProduct.Id });

            return result > 0;
        }

        public async Task<IEnumerable<AcquisitionProduct>> GetAllProducts()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Type, 
                                Status
                        FROM acquisitionproducts";

            return await db.QueryAsync<AcquisitionProduct>(sql, new { });
        }

        public async Task<AcquisitionProduct> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Type, 
                                Status
                        FROM acquisitionproducts
                        WHERE acquisitionproducts.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionProduct>(sql, new { Id = id });
        }

        public async Task<bool> InsertProduct(AcquisitionProduct acquisitionProduct)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionproducts(Name,
                                                        Type)
                        VALUES (@Name,
                                @Type)";

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionProduct.Name,
                acquisitionProduct.Type
            });

            return result > 0;
        }

        public async Task<bool> UpdateProduct(AcquisitionProduct acquisitionProduct)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionproducts SET ");

            if (!string.IsNullOrWhiteSpace(acquisitionProduct.Name))
                sqlBuilder.Append("Name = @Name,");

            if (acquisitionProduct.Type != null)
                sqlBuilder.Append("Type = @Type,");

            if (acquisitionProduct.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionProduct.Name,
                acquisitionProduct.Type,
                acquisitionProduct.Status,
                acquisitionProduct.Id
            });

            return result > 0;
        }

    }
}
