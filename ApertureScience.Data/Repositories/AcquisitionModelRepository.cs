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
    public class AcquisitionModelRepository : IAcquisitionModelRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionModelRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> DeleteModel(AcquisitionModel acquisitionModel)
        {
            var db = dbConnection();

            var sql = @" UPDATE acquisitionmodels
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { acquisitionModel.Status, acquisitionModel.Id });

            return result > 0;
        }

        public async Task<IEnumerable<AcquisitionModel>> GetAllModels()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Status
                        FROM acquisitionmodels";

            return await db.QueryAsync<AcquisitionModel>(sql, new { });
        }

        public async Task<AcquisitionModel> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, 
                                Name, 
                                Status
                        FROM acquisitionmodels
                        WHERE acquisitionmodels.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionModel>(sql, new { Id = id });
        }

        public async Task<bool> InsertModel(AcquisitionModel acquisitionModel)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionmodels(Name)
                        VALUES (@Name)";

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionModel.Name
            });

            return result > 0;
        }

        public async Task<bool> UpdateModel(AcquisitionModel acquisitionModel)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionmodels SET ");

            if (!string.IsNullOrWhiteSpace(acquisitionModel.Name))
                sqlBuilder.Append("Name = @Name,");

            if (acquisitionModel.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionModel.Name,
                acquisitionModel.Status,
                acquisitionModel.Id
            });

            return result > 0;
        }
    }
}
