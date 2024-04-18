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
    public class AcquisitionEntryBySupplierHRepository : IAcquisitionEntryBySupplierHRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionEntryBySupplierHRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<AcquisitionEntryBySupplierH>> GetAllEntryBySupplierH()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriessuppliersh.Id, 
                                acquisitionentriessuppliersh.Date, 
                                acquisitionentriessuppliersh.Type,
                                acquisitionentriessuppliersh.Elaborated,
                                acquisitionentriessuppliersh.Observations,
                                acquisitionentriessuppliersh.Status,
                                acquisitionentriessuppliersh.IdSuppliers,
                                suppliers.Tradename
                        FROM acquisitionentriessuppliersh
                        INNER JOIN suppliers ON suppliers.Id = acquisitionentriessuppliersh.IdSuppliers";

            return await db.QueryAsync<AcquisitionEntryBySupplierH>(sql, new { });
        }

        public async Task<IEnumerable<AcquisitionEntryBySupplierH>> GetByDates(DateTime startDate, DateTime endDate)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriessuppliersh.Id, 
                                acquisitionentriessuppliersh.Date, 
                                acquisitionentriessuppliersh.Type,
                                acquisitionentriessuppliersh.Elaborated,
                                acquisitionentriessuppliersh.Observations,
                                acquisitionentriessuppliersh.Status,
                                acquisitionentriessuppliersh.IdSuppliers,
                                suppliers.Tradename
                        FROM acquisitionentriessuppliersh
                        INNER JOIN suppliers ON suppliers.Id = acquisitionentriessuppliersh.IdSuppliers
                        WHERE acquisitionentriessuppliersh.Date >= @StartDate AND 
                                acquisitionentriessuppliersh.Date <= @EndDate";

            return await db.QueryAsync<AcquisitionEntryBySupplierH>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<AcquisitionEntryBySupplierH> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriessuppliersh.Id, 
                                acquisitionentriessuppliersh.Date, 
                                acquisitionentriessuppliersh.Type,
                                acquisitionentriessuppliersh.Elaborated,
                                acquisitionentriessuppliersh.Observations,
                                acquisitionentriessuppliersh.Status,
                                acquisitionentriessuppliersh.IdSuppliers,
                                suppliers.Tradename
                        FROM acquisitionentriessuppliersh
                        INNER JOIN suppliers ON suppliers.Id = acquisitionentriessuppliersh.IdSuppliers
                        WHERE acquisitionentriessuppliersh.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionEntryBySupplierH>(sql, new { Id = id });
        }

        public async Task<int> InsertEntryBySupplierH(AcquisitionEntryBySupplierH acquisitionEntryH)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionentriessuppliersh(  Date, 
                                                            Type,
                                                            Elaborated,
                                                            Observations,
                                                            IdSuppliers
                                                            )
                        VALUES (                @Date, 
                                                @Type,
                                                @Elaborated,
                                                @Observations,
                                                @IdSuppliers
                                                );
                        SELECT LAST_INSERT_ID();";

            var insertedId = await db.ExecuteScalarAsync<int>(sql, new
            {
                acquisitionEntryH.Date,
                acquisitionEntryH.Type,
                acquisitionEntryH.Elaborated,
                acquisitionEntryH.Observations,
                acquisitionEntryH.IdSuppliers
            });

            return insertedId;
        }
    }
}
