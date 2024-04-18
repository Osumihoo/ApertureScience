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
    public class BusinessnameRepository : IBusinessnameRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public BusinessnameRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteBusinessname(Businessname businessname)
        {
            var db = dbConnection();

            var sql = @" UPDATE businessname
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { businessname.Status, businessname.Id });

            return result > 0;
        }

        public async Task<IEnumerable<Businessname>> GetAllBusinessname()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Name, Status
                        FROM businessname";

            return await db.QueryAsync<Businessname>(sql, new { });
        }

        public async Task<Businessname> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Name, Status
                        FROM businessname
                        WHERE Id = @Id";

            return await db.QueryFirstOrDefaultAsync<Businessname>(sql, new { Id = id });
        }

        public async Task<bool> InsertBusinessname(Businessname businessname)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO businessname(Type)
                        VALUES (@Name)";

            var result = await db.ExecuteAsync(sql, new
            { businessname.Name });

            return result > 0;
        }

        public async Task<bool> UpdateBusinessname(Businessname businessname)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE businessname SET ");

            if (!string.IsNullOrWhiteSpace(businessname.Name))
                sqlBuilder.Append("Name = @Name,");

            if (businessname.Status != null)
                sqlBuilder.Append("Status = @Status,");

            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; 

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            { businessname.Name, businessname.Status, businessname.Id });

            return result > 0;
        }
    }
}
