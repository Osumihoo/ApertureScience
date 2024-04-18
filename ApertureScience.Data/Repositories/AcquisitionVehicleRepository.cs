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
    public class AcquisitionVehicleRepository : IAcquisitionVehicleRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionVehicleRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteVehicle(AcquisitionVehicle acquisitionVehicle)
        {
            var db = dbConnection();

            var sql = @" UPDATE acquisitionvehicles
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { acquisitionVehicle.Status, acquisitionVehicle.Id });

            return result > 0;
        }

        public async Task<IEnumerable<AcquisitionVehicle>> GetAllVehicles()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Plate, 
                                Status
                        FROM acquisitionvehicles";

            return await db.QueryAsync<AcquisitionVehicle>(sql, new { });
        }

        public async Task<AcquisitionVehicle> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Plate, 
                                Status
                        FROM acquisitionvehicles
                        WHERE acquisitionvehicles.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionVehicle>(sql, new { Id = id });
        }

        public async Task<bool> InsertVehicle(AcquisitionVehicle acquisitionVehicle)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionvehicles(Name, Plate)
                        VALUES (@Name, @Plate)";

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionVehicle.Name,
                acquisitionVehicle.Plate
            });

            return result > 0;
        }

        public async Task<bool> UpdateVehicle(AcquisitionVehicle acquisitionVehicle)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionvehicles SET ");

            if (!string.IsNullOrWhiteSpace(acquisitionVehicle.Name))
                sqlBuilder.Append("Name = @Name,");

            if (!string.IsNullOrWhiteSpace(acquisitionVehicle.Plate))
                sqlBuilder.Append("Plate = @Plate,");

            if (acquisitionVehicle.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionVehicle.Name,
                acquisitionVehicle.Plate,
                acquisitionVehicle.Status,
                acquisitionVehicle.Id
            });

            return result > 0;
        }
    }
}
