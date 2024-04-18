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
    public class AcquisitionReleaseHRepository : IAcquisitionReleaseHRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionReleaseHRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<AcquisitionReleaseH>> GetAllReleaseH()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionreleaseh.Id, 
                                acquisitionreleaseh.Date, 
                                acquisitionreleaseh.Type,
                                acquisitionreleaseh.Elaborated,
                                acquisitionreleaseh.Observations,
                                acquisitionreleaseh.Status,
                                acquisitionreleaseh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitionreleaseh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitionreleaseh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitionreleaseh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitionreleaseh.IdDepartment, 
                                departments.Name AS DepartmentName
                        FROM acquisitionreleaseh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitionreleaseh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitionreleaseh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitionreleaseh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitionreleaseh.IdAcVehicle
                        INNER JOIN departments ON departments.Id = acquisitionreleaseh.IdDepartment
                        ORDER BY acquisitionreleaseh.Id";

            return await db.QueryAsync<AcquisitionReleaseH>(sql, new { });
        }

        public async Task<AcquisitionReleaseH> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionreleaseh.Id, 
                                acquisitionreleaseh.Date, 
                                acquisitionreleaseh.Type,
                                acquisitionreleaseh.Elaborated,
                                acquisitionreleaseh.Observations,
                                acquisitionreleaseh.Status,
                                acquisitionreleaseh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitionreleaseh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitionreleaseh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitionreleaseh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitionreleaseh.IdDepartment, 
                                departments.Name AS DepartmentName,
                                acquisitionreleaseh.ReceptionCode 
                        FROM acquisitionreleaseh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitionreleaseh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitionreleaseh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitionreleaseh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitionreleaseh.IdAcVehicle
                        INNER JOIN departments ON departments.Id = acquisitionreleaseh.IdDepartment
                        WHERE acquisitionreleaseh.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionReleaseH>(sql, new { Id = id });
        }
        public async Task<IEnumerable<AcquisitionReleaseH>> GetByDates(DateTime startDate, DateTime endDate)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionreleaseh.Id, 
                                acquisitionreleaseh.Date, 
                                acquisitionreleaseh.Type,
                                acquisitionreleaseh.Elaborated,
                                acquisitionreleaseh.Observations,
                                acquisitionreleaseh.Status,
                                acquisitionreleaseh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitionreleaseh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitionreleaseh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitionreleaseh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitionreleaseh.IdDepartment, 
                                departments.Name AS DepartmentName
                        FROM acquisitionreleaseh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitionreleaseh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitionreleaseh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitionreleaseh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitionreleaseh.IdAcVehicle
                        INNER JOIN departments ON departments.Id = acquisitionreleaseh.IdDepartment
                        WHERE acquisitionreleaseh.Date >= @StartDate AND 
                                acquisitionreleaseh.Date <= @EndDate
                        ORDER BY acquisitionreleaseh.Id";

            return await db.QueryAsync<AcquisitionReleaseH>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> InsertReleaseH(AcquisitionReleaseH acquisitionReleaseH)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionreleaseh(  Date, 
                                                            Type,
                                                            Elaborated,
                                                            Observations,
                                                            Status,
                                                            IdAddressRelease,
                                                            IdAddressEntry,
                                                            IdAcCarrier,
                                                            IdAcVehicle,
                                                            IdDepartment)
                        VALUES (                @Date, 
                                                @Type,
                                                @Elaborated,
                                                @Observations,
                                                @Status,
                                                @IdAddressRelease,
                                                @IdAddressEntry,
                                                @IdAcCarrier,
                                                @IdAcVehicle,
                                                @IdDepartment);
                        SELECT LAST_INSERT_ID();";

            var insertedId = await db.ExecuteScalarAsync<int>(sql, new
            {
                acquisitionReleaseH.Date,
                acquisitionReleaseH.Type,
                acquisitionReleaseH.Elaborated,
                acquisitionReleaseH.Observations,
                acquisitionReleaseH.Status,
                acquisitionReleaseH.IdAddressRelease,
                acquisitionReleaseH.IdAddressEntry,
                acquisitionReleaseH.IdAcCarrier,
                acquisitionReleaseH.IdAcVehicle,
                acquisitionReleaseH.IdDepartment
            });

            return insertedId;
        }

        public async Task<bool> AuthorizeReleaseH(int id, int status)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionreleaseh SET ");

            if (status == 1)
                sqlBuilder.Append("Status = @Status,");


            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                status,
                id
            });

            return result > 0;
        }

    }
}
