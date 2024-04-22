using ApertureScience.Model;
using Dapper;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Relational;

namespace ApertureScience.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public UserRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteUser(User user)
        {
            var db = dbConnection();

            var sql = @" UPDATE users
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { user.Status, user.Id });

            return result > 0;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var db = dbConnection();
            
            //var sql = @" SELECT Id, Name, LastNameP, LastNameM, Email, Username, Password, Status, IdRole, IdDepartment
            //                FROM users";
            var sql = @" SELECT users.Id, users.Name, users.LastNameP, users.LastNameM, users.Email, users.Username, users.Password, users.Status, users.IdRole, roles.Type AS RolName, users.IdDepartment, departments.Name AS DepartmentName
                            FROM users
                            INNER JOIN departments ON departments.Id = users.IdDepartment
                            INNER JOIN roles ON roles.Id = users.IdRole";
            //var sql = @"SELECT * FROM users";

            return await db.QueryAsync<User>(sql, new { });
        }

        public async Task<User> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT users.Id, users.Name, users.LastNameP, users.LastNameM, users.Email, users.Username, users.Password, users.Status, users.IdRole, roles.Type AS RolName, users.IdDepartment, departments.Name AS DepartmentName
                            FROM users
                            INNER JOIN departments ON departments.Id = users.IdDepartment
                            INNER JOIN roles ON roles.Id = users.IdRole
                        WHERE users.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User> GetByEmail(string email)
        {
            var db = dbConnection();

            var sql = "SELECT Name, LastNameP, LastNameM, IdRole, IdDepartment " +
                "FROM users " +
                "WHERE Email = @Email";

            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User> InsertUser(User user)
        {
            var db = dbConnection();

            string hashedPassword = PasswordHelper.HashPassword(user.Password);

            var sql = @" INSERT INTO users(Name, LastNameP, LastNameM, Email, Username, Password, IdRole, IdDepartment)
                        VALUES (@Name, @LastNameP, @LastNameM, @Email, @Username, @Password, @IdRole, @IdDepartment)";

            var result = await db.ExecuteAsync(sql, new
            { user.Name, user.LastNameP, user.LastNameM, user.Email, user.Username, Password = hashedPassword, user.IdRole, user.IdDepartment });

            if (result > 0)
            {
                //var insertedUser = await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE Email = @Email", new { Email = user.Email });
                var insertedUser = await GetByEmail(user.Email);

                return insertedUser;
            }
            else
            {
                return null; // O algún otro manejo de error si es necesario
            }

            //return await db.QueryFirstOrDefaultAsync<User>(sql, new { Name = user.Name, LastNameP = user.LastNameP, LastNameM = user.LastNameM, Email = user.Email, Username = user.Username, Password = hashedPassword, IdRole = user.IdRole, IdDepartment = user.IdDepartment });
            //return result > 0; 
        }

        public async Task<bool> UpdateUser(User user)
        {
            string hashedPassword = null;

            var db = dbConnection();


            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE users SET ");

            if (!string.IsNullOrWhiteSpace(user.Name))
                sqlBuilder.Append("Name = @Name,");
            
            if (!string.IsNullOrWhiteSpace(user.LastNameP))
                sqlBuilder.Append("LastNameP = @LastNameP,");
            
            if (!string.IsNullOrWhiteSpace(user.LastNameM))
                sqlBuilder.Append("LastNameM = @LastNameM,");
            
            if (!string.IsNullOrWhiteSpace(user.Email))
                sqlBuilder.Append("Email = @Email,");
            
            if (!string.IsNullOrWhiteSpace(user.Username))
                sqlBuilder.Append("Username = @Username,"); 
            
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                sqlBuilder.Append("Password = @Password,");
                hashedPassword = PasswordHelper.HashPassword(user.Password);
            }    

            if (user.Status != null )
                sqlBuilder.Append("Status = @Status,");
            
            if (user.IdRole != null && user.IdRole != 0)
                sqlBuilder.Append("IdRole = @IdRole,");            
            
            if (user.IdDepartment != null && user.IdDepartment != 0)
                sqlBuilder.Append("IdDepartment = @IdDepartment,");
       

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
            {
                sqlBuilder.Length--; // Elimina la última coma
            }

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();           

                    var result = await db.ExecuteAsync(sql, new
            { user.Name, user.LastNameP, user.LastNameM, user.Email, user.Username, Password = hashedPassword, user.Status, user.IdRole, user.IdDepartment, user.Id });

            return result > 0;
        }

        
    }
}
