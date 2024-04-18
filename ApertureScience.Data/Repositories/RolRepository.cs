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
    public class RolRepository : IRolRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public RolRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteRol(Rol rol)
        {
            var db = dbConnection();

            var sql = @" UPDATE roles
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { rol.Status, rol.Id });

            return result > 0;
        }

        public async Task<IEnumerable<Rol>> GetAllRoles()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Type, Status
                        FROM roles";

            return await db.QueryAsync<Rol>(sql, new { });
        }

        public async Task<Rol> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Type, Status
                        FROM roles
                        WHERE Id = @Id";

            return await db.QueryFirstOrDefaultAsync<Rol>(sql, new { Id = id });
        }

        public async Task<bool> InsertRol(Rol rol)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO roles(Type)
                        VALUES (@Type)";

            var result = await db.ExecuteAsync(sql, new
            { rol.Type });

            return result > 0;
        }

        public async Task<bool> UpdateRol(Rol rol)
        {
            var db = dbConnection();

            //var sql = @" UPDATE roles
            //                SET Type = @Type,
            //                    Status = @Status
            //                WHERE Id = @Id";

            //if (string.IsNullOrWhiteSpace(rol.Type))
            //    sql = sql.Replace("Type = @Type,", "");
            //if (rol.Status == 0 || rol.Status == null)
            //    sql = sql.Replace(", Status = @Status", "");

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE roles SET ");

            if (!string.IsNullOrWhiteSpace(rol.Type))
                sqlBuilder.Append("Type = @Type,");

            if (rol.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            { rol.Type, rol.Status, rol.Id });

            return result > 0;
        }
    }
}
