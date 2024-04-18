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
    public class AcquisitionBrandRepository : IAcquisitionBrandRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionBrandRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> DeleteBrand(AcquisitionBrand acquisitionBrand)
        {
            var db = dbConnection();

            var sql = @" UPDATE acquisitionbrands
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { acquisitionBrand.Status, acquisitionBrand.Id });

            return result > 0;
        }

        public async Task<IEnumerable<AcquisitionBrand>> GetAllBrands()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Status
                        FROM acquisitionbrands";

            return await db.QueryAsync<AcquisitionBrand>(sql, new { });
        }

        public async Task<AcquisitionBrand> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Status
                        FROM acquisitionbrands
                        WHERE acquisitionbrands.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionBrand>(sql, new { Id = id });
        }

        public async Task<bool> InsertBrand(AcquisitionBrand acquisitionBrand)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionbrands(Name)
                        VALUES (@Name)";

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionBrand.Name
            });

            return result > 0;
        }

        public async Task<bool> UpdateBrand(AcquisitionBrand acquisitionBrand)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionbrands SET ");

            if (!string.IsNullOrWhiteSpace(acquisitionBrand.Name))
                sqlBuilder.Append("Name = @Name,");

            if (acquisitionBrand.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionBrand.Name,
                acquisitionBrand.Status,
                acquisitionBrand.Id
            });

            return result > 0;
        }
    }
}
