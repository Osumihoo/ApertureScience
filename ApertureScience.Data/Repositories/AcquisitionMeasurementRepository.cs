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
    public class AcquisitionMeasurementRepository : IAcquisitionMeasurementRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionMeasurementRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> DeleteMeasurement(AcquisitionMeasurement acquisitionMeasurement)
        {
            var db = dbConnection();

            var sql = @" UPDATE acquisitionmeasurements
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { acquisitionMeasurement.Status, acquisitionMeasurement.Id });

            return result > 0;
        }

        public async Task<IEnumerable<AcquisitionMeasurement>> GetAllMeasurements()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Status
                        FROM acquisitionmeasurements";

            return await db.QueryAsync<AcquisitionMeasurement>(sql, new { });
        }

        public async Task<AcquisitionMeasurement> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Status
                        FROM acquisitionmeasurements
                        WHERE acquisitionmeasurements.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionMeasurement>(sql, new { Id = id });
        }

        public async Task<bool> InsertMeasurement(AcquisitionMeasurement acquisitionMeasurement)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionmeasurements(Name)
                        VALUES (@Name)";

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionMeasurement.Name
            });

            return result > 0;
        }

        public async Task<bool> UpdateMeasurement(AcquisitionMeasurement acquisitionMeasurement)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionmeasurements SET ");

            if (!string.IsNullOrWhiteSpace(acquisitionMeasurement.Name))
                sqlBuilder.Append("Name = @Name,");

            if (acquisitionMeasurement.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionMeasurement.Name,
                acquisitionMeasurement.Status,
                acquisitionMeasurement.Id
            });

            return result > 0;
        }
    }
}
