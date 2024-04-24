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
    public class AcquisitionItemByDepartmentRepository : IAcquisitionItemByDepartmentRepository
    {

        private readonly MySQLConfiguration _connectionString;

        public AcquisitionItemByDepartmentRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<AcquisitionItemByDepartment>> GetAllItemByDepartments()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionitembydepartment.Id, 
                                acquisitionitembydepartment.CodeAcSupplies, 
                                acquisitionitembydepartment.OnHand, 
                                acquisitionitembydepartment.Remaining, 
                                acquisitionitembydepartment.Total, 
                                acquisitionitembydepartment.IdDepartment, 
                                departments.Name AS DepartmentName
                        FROM acquisitionitembydepartment
                        INNER JOIN departments ON departments.Id = acquisitionitembydepartment.IdDepartment";

            return await db.QueryAsync<AcquisitionItemByDepartment>(sql, new { });
        }

        public async Task<AcquisitionItemByDepartment> GetByDepartment(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionitembydepartment.Id, 
                                acquisitionitembydepartment.CodeAcSupplies, 
                                acquisitionitembydepartment.OnHand, 
                                acquisitionitembydepartment.Remaining, 
                                acquisitionitembydepartment.Total, 
                                acquisitionitembydepartment.IdDepartment, 
                                departments.Name AS DepartmentName
                        FROM acquisitionitembydepartment
                        INNER JOIN departments ON departments.Id = acquisitionitembydepartment.IdDepartment
                        WHERE acquisitionitembydepartment.IdDepartment = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionItemByDepartment>(sql, new { Id = id });
        }

        public async Task<bool> InsertItemByDepartment(AcquisitionItemByDepartment acquisitionItemByDepartment)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionitembydepartment(   CodeAcSupplies,
                                                                    OnHand,
                                                                    Remaining,
                                                                    Total,
                                                                    IdDepartment)
                        VALUES (@CodeAcSupplies
                                @OnHand,
                                @Remaining,
                                @Total,
                                @IdDepartment
                                )";

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionItemByDepartment.CodeAcSupplies,
                acquisitionItemByDepartment.OnHand,
                acquisitionItemByDepartment.Remaining,
                acquisitionItemByDepartment.Total,
                acquisitionItemByDepartment.IdDepartment
            });

            return result > 0;
        }

        public async Task<bool> UpdateItemByDepartment(AcquisitionItemByDepartment acquisitionItemByDepartment)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionitembydepartment SET ");

            if (acquisitionItemByDepartment.OnHand != null)
                sqlBuilder.Append("OnHand = @OnHand,");

            if (acquisitionItemByDepartment.Remaining != null)
                sqlBuilder.Append("Remaining = @Remaining,");

            if (acquisitionItemByDepartment.Total != null)
                sqlBuilder.Append("Total = @Total,");

            if (!string.IsNullOrWhiteSpace(acquisitionItemByDepartment.CodeAcSupplies))
                sqlBuilder.Append("CodeAcSupplies = @CodeAcSupplies,");

            if (acquisitionItemByDepartment.IdDepartment != null)
                sqlBuilder.Append("IdDepartment = @IdDepartment,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                acquisitionItemByDepartment.CodeAcSupplies,
                acquisitionItemByDepartment.OnHand,
                acquisitionItemByDepartment.Remaining,
                acquisitionItemByDepartment.Total,
                acquisitionItemByDepartment.IdDepartment,
                acquisitionItemByDepartment.Id
            });

            return result > 0;
        }
    }
}
