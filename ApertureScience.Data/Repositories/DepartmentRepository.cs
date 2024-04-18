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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public DepartmentRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteDepartment(Department department)
        {
            var db = dbConnection();

            var sql = @" UPDATE departments
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { department.Status, department.Id });

            return result > 0;
        }

        public async Task<IEnumerable<Department>> GetAllDepartments()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Name, Code, Type, Status, IdAddress
                        FROM departments";

            return await db.QueryAsync<Department>(sql, new { });
        }

        public async Task<Department> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT departments.Id, departments.Name, departments.Code, departments.Type, departments.Status, departments.IdAddress, addresses.Name AS AddressName
                        FROM departments
                        INNER JOIN addresses ON addresses.Id = departments.IdAddress
                        WHERE departments.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<Department>(sql, new { Id = id });
        }

        public async Task<bool> InsertDepartment(Department department)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO departments(Name, Code, Type, IdAddress)
                        VALUES (@Name, @Code, @Type, @IdAddress)";

            var result = await db.ExecuteAsync(sql, new
            { department.Name, department.Code, department.Type, department.IdAddress });

            return result > 0;
        }

        public async Task<bool> UpdateDepartment(Department department)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE departments SET ");

            if (!string.IsNullOrWhiteSpace(department.Name))
                sqlBuilder.Append("Name = @Name,");

            if (!string.IsNullOrWhiteSpace(department.Code))
                sqlBuilder.Append("Code = @Code,");

            if (!string.IsNullOrWhiteSpace(department.Type))
                sqlBuilder.Append("Type = @Type,");

            if (department.Status != null)
                sqlBuilder.Append("Status = @Status,");

            if (department.IdAddress != null && department.IdAddress != 0)
                sqlBuilder.Append("IdAddress = @IdAddress,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
            {
                sqlBuilder.Length--; // Elimina la última coma
            }

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            { department.Name, department.Code, department.Type, department.Status, department.IdAddress, department.Id });

            return result > 0;
        }

    }
}
