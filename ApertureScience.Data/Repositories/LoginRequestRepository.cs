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
    public class LoginRequestRepository : ILoginRequestRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public LoginRequestRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<LoginRequest> GetByEmail(LoginRequest loginRequest)
        {
            var db = dbConnection();

            var sql = @" SELECT Email, Password, Id, Name, LastNameP, LastNameM, IdRole, IdDepartment
                        FROM users
                        WHERE Email = @Email";

            //var result = await db.ExecuteAsync(sql, new
            //{ loginRequest.Email, loginRequest.Password, loginRequest.Name, loginRequest.LastNameP, loginRequest.LastNameM, loginRequest.IdRole, loginRequest.IdDepartment});

            return await db.QueryFirstOrDefaultAsync<LoginRequest>(sql, new { Email = loginRequest.Email });
        }

    }
}
