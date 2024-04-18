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
    public class AddressRepository : IAddressRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AddressRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }


        public async Task<bool> DeleteAddress(Address address)
        {
            var db = dbConnection();

            var sql = @" UPDATE addresses
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { address.Status, address.Id });

            return result > 0;
        }

        public async Task<IEnumerable<Address>> GetAllAddresses()
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Name, Street, Neighborhood, City, PC, State, Country, Num, IntNum, Status
                        FROM addresses";

            return await db.QueryAsync<Address>(sql, new { });
        }

        public async Task<Address> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT Id, Name, Street, Neighborhood, City, PC, State, Country, Num, IntNum, Status
                        FROM addresses
                        WHERE Id = @Id";

            return await db.QueryFirstOrDefaultAsync<Address>(sql, new { Id = id });
        }

        public async Task<bool> InsertAddress(Address address)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO addresses(Name, Street, Neighborhood, City, PC, State, Country, Num, IntNum)
                        VALUES (@Name, @Street, @Neighborhood, @City, @PC, @State, @Country, @Num, @IntNum)";

            var result = await db.ExecuteAsync(sql, new
            {   address.Name, 
                address.Street, 
                address.Neighborhood, 
                address.City, 
                address.PC, 
                address.State, 
                address.Country, 
                address.Num,
                address.IntNum,
                address.Status
            });

            return result > 0;
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE addresses SET ");

            if (!string.IsNullOrWhiteSpace(address.Name))
                sqlBuilder.Append("Name = @Name,");

            if (!string.IsNullOrWhiteSpace(address.Street))
                sqlBuilder.Append("Street = @Street,");

            if (!string.IsNullOrWhiteSpace(address.Neighborhood))
                sqlBuilder.Append("Neighborhood = @Neighborhood,");

            if (!string.IsNullOrWhiteSpace(address.City))
                sqlBuilder.Append("City = @City,");

            if (!string.IsNullOrWhiteSpace(address.PC))
                sqlBuilder.Append("PC = @PC,");

            if (!string.IsNullOrWhiteSpace(address.State))
                sqlBuilder.Append("State = @State,");

            if (!string.IsNullOrWhiteSpace(address.Country))
                sqlBuilder.Append("Country = @Country,");

            if (!string.IsNullOrWhiteSpace(address.Num))
                sqlBuilder.Append("Num = @Num,");

            if (!string.IsNullOrWhiteSpace(address.IntNum))
                sqlBuilder.Append("IntNum = @IntNum,");

            if (address.Status != null )
                sqlBuilder.Append("Status = @Status,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                address.Name,
                address.Street,
                address.Neighborhood,
                address.City,
                address.PC,
                address.State,
                address.Country,
                address.Num,
                address.IntNum,
                address.Status, 
                address.Id });

            return result > 0;
        }
    }
}
