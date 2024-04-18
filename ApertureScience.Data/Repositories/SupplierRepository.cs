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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public SupplierRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> DeleteSupplier(Supplier supplier)
        {
            var db = dbConnection();

            var sql = @" UPDATE suppliers
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { supplier.Status, supplier.Id });

            return result > 0;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliers()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Tradename, Businessname, Code, RFC, Contact, Email, Status
                        FROM suppliers";

            return await db.QueryAsync<Supplier>(sql, new { });
        }

        public async Task<Supplier> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Tradename, Businessname, Code, RFC, Contact, Email, Status
                        FROM suppliers
                        WHERE Id = @Id";

            return await db.QueryFirstOrDefaultAsync<Supplier>(sql, new { Id = id });
        }

        public async Task<bool> InsertSupplier(Supplier supplier)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO suppliers(Tradename, Businessname, Code, RFC, Contact, Email)
                        VALUES (@Tradename, @Businessname, @Code, @RFC, @Contact, @Email)";

            var result = await db.ExecuteAsync(sql, new
            {   supplier.Tradename,
                supplier.Businessname,
                supplier.Code, 
                supplier.RFC, 
                supplier.Contact, 
                supplier.Email });

            return result > 0;
        }

        public async Task<bool> UpdateSupplier(Supplier supplier)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE suppliers SET ");

            if (!string.IsNullOrWhiteSpace(supplier.Tradename))
                sqlBuilder.Append("Tradename = @Tradename,");

            if (!string.IsNullOrWhiteSpace(supplier.Businessname))
                sqlBuilder.Append("Businessname = @Businessname,");

            if (!string.IsNullOrWhiteSpace(supplier.Code))
                sqlBuilder.Append("Code = @Code,");

            if (!string.IsNullOrWhiteSpace(supplier.RFC))
                sqlBuilder.Append("RFC = @RFC,");

            if (!string.IsNullOrWhiteSpace(supplier.Contact))
                sqlBuilder.Append("Contact = @Contact,");

            if (!string.IsNullOrWhiteSpace(supplier.Email))
                sqlBuilder.Append("Email = @Email,");

            if (supplier.Status != null)
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {   supplier.Tradename,
                supplier.Businessname,
                supplier.Code, 
                supplier.RFC, 
                supplier.Contact, 
                supplier.Email, 
                supplier.Status, 
                supplier.Id 
            });

            return result > 0;
        }
    }
}
