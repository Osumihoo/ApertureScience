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
    public class AcquisitionCarrierRepository : IAcquisitionCarrierRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionCarrierRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteCarrier(AcquisitionCarrier acquisitionCarrier)
        {
            var db = dbConnection();

            var sql = @" UPDATE acquisitioncarriers
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { acquisitionCarrier.Status, acquisitionCarrier.Id });

            return result > 0;
        }

        public async Task<IEnumerable<AcquisitionCarrier>> GetAllCarriers()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                LastNameP, 
                                LastNameM, 
                                Status
                        FROM acquisitioncarriers";

            return await db.QueryAsync<AcquisitionCarrier>(sql, new { });
        }

        public async Task<AcquisitionCarrier> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                LastNameP, 
                                LastNameM, 
                                Status
                        FROM acquisitioncarriers
                        WHERE acquisitioncarriers.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionCarrier>(sql, new { Id = id });
        }

        public async Task<bool> InsertCarrier(AcquisitionCarrier acquisitionCarrier)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitioncarriers(Name, LastNameP, LastNameM)
                        VALUES (@Name, @LastNameP, @LastNameM)";

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionCarrier.Name,
                acquisitionCarrier.LastNameP,
                acquisitionCarrier.LastNameM
            });

            return result > 0;
        }

        public async Task<bool> UpdateCarrier(AcquisitionCarrier acquisitionCarrier)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitioncarriers SET ");

            if (!string.IsNullOrWhiteSpace(acquisitionCarrier.Name))
                sqlBuilder.Append("Name = @Name,");

            if (!string.IsNullOrWhiteSpace(acquisitionCarrier.LastNameP))
                sqlBuilder.Append("LastNameP = @LastNameP,");

            if (!string.IsNullOrWhiteSpace(acquisitionCarrier.LastNameM))
                sqlBuilder.Append("LastNameM = @LastNameM,");

            if (acquisitionCarrier.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionCarrier.Name,
                acquisitionCarrier.LastNameP,
                acquisitionCarrier.LastNameM,
                acquisitionCarrier.Status,
                acquisitionCarrier.Id
            });

            return result > 0;
        }
    }
}
