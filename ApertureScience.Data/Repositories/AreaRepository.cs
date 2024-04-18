using ApertureScience.Model;
using Dapper;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AreaRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteArea(Area area)
        {
            var db = dbConnection();

            var sql = @" UPDATE areas
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { area.Status, area.Id });

            return result > 0;
        }

        public async Task<IEnumerable<Area>> GetAllAreas()
        {
            var db = dbConnection();

            var sql = @" SELECT areas.Id, areas.Name, areas.Status, areas.IdAddress, addresses.Street AS NameAddress, addresses.Num AS NumAddress
                        FROM areas
                        INNER JOIN addresses ON addresses.Id = areas.IdAddress";

            return await db.QueryAsync<Area>(sql, new { });
        }

        public async Task<Area> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT areas.Id, areas.Name, areas.Status, areas.IdAddress, addresses.Street AS NameAddress, addresses.Num AS NumAddress
                        FROM areas
                        INNER JOIN addresses ON addresses.Id = areas.IdAddress
                        WHERE areas.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<Area>(sql, new { Id = id });
        }

        public async Task<bool> InsertArea(Area area)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO areas(Name, IdAddress)
                        VALUES (@Name, @IdAddress)";

            var result = await db.ExecuteAsync(sql, new
            { area.Name, area.IdAddress });

            return result > 0;
        }

        public async Task<bool> UpdateArea(Area area)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE areas SET ");

            if (!string.IsNullOrWhiteSpace(area.Name))
                sqlBuilder.Append("Name = @Name,");

            if (area.Status != null)
                sqlBuilder.Append("Status = @Status,");

            if (area.IdAddress != null && area.IdAddress != 0)
                sqlBuilder.Append("IdAddress = @IdAddress,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            { area.Name, area.Status, area.IdAddress, area.Id });

            return result > 0;
        }

    }
}
