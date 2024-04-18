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
    public class AcquisitionEntryHRepository : IAcquisitionEntryHRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionEntryHRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<AcquisitionEntryH>> GetAllEntryH()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriesh.Id, 
                                acquisitionentriesh.Date, 
                                acquisitionentriesh.Type,
                                acquisitionentriesh.Elaborated,
                                acquisitionentriesh.Observations,
                                acquisitionentriesh.Status,
                                acquisitionentriesh.IdAcReleaseH, 
                                acquisitionentriesh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitionentriesh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitionentriesh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitionentriesh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitionentriesh.IdDepartment, 
                                departments.Name AS DepartmentName
                        FROM acquisitionentriesh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitionentriesh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitionentriesh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitionentriesh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitionentriesh.IdAcVehicle
                        INNER JOIN departments ON departments.Id = acquisitionentriesh.IdDepartment
                        ORDER BY acquisitionentriesh.Id";

            return await db.QueryAsync<AcquisitionEntryH>(sql, new { });
        }

        public async Task<AcquisitionEntryH> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriesh.Id, 
                                acquisitionentriesh.Date, 
                                acquisitionentriesh.Type,
                                acquisitionentriesh.Elaborated,
                                acquisitionentriesh.Observations,
                                acquisitionentriesh.ReceptionCode,
                                acquisitionentriesh.Status,
                                acquisitionentriesh.IdAcReleaseH, 
                                acquisitionentriesh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitionentriesh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitionentriesh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitionentriesh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitionentriesh.IdDepartment, 
                                departments.Name AS DepartmentName
                        FROM acquisitionentriesh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitionentriesh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitionentriesh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitionentriesh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitionentriesh.IdAcVehicle
                        INNER JOIN departments ON departments.Id = acquisitionentriesh.IdDepartment
                        WHERE acquisitionentriesh.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionEntryH>(sql, new { Id = id });
        }
        public async Task<IEnumerable<AcquisitionEntryH>> GetByDates(DateTime startDate, DateTime endDate)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriesh.Id, 
                                acquisitionentriesh.Date, 
                                acquisitionentriesh.Type,
                                acquisitionentriesh.Elaborated,
                                acquisitionentriesh.Observations,
                                acquisitionentriesh.Status,
                                acquisitionentriesh.IdAcReleaseH, 
                                acquisitionentriesh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitionentriesh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitionentriesh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitionentriesh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitionentriesh.IdDepartment, 
                                departments.Name AS DepartmentName
                        FROM acquisitionentriesh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitionentriesh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitionentriesh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitionentriesh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitionentriesh.IdAcVehicle
                        INNER JOIN departments ON departments.Id = acquisitionentriesh.IdDepartment
                        WHERE acquisitionentriesh.Date >= @StartDate AND 
                                acquisitionentriesh.Date <= @EndDate
                        ORDER BY acquisitionentriesh.Id";

            return await db.QueryAsync<AcquisitionEntryH>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> InsertEntryH(AcquisitionEntryH acquisitionEntryH)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionentriesh(  Date, 
                                                            Type,
                                                            Elaborated,
                                                            Observations,
                                                            Status,
                                                            IdAcReleaseH,
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
                                                @IdAcReleaseH,
                                                @IdAddressRelease,
                                                @IdAddressEntry,
                                                @IdAcCarrier,
                                                @IdAcVehicle,
                                                @IdDepartment);
                        SELECT LAST_INSERT_ID();";

            var insertedId = await db.ExecuteScalarAsync<int>(sql, new
            {
                acquisitionEntryH.Date,
                acquisitionEntryH.Type,
                acquisitionEntryH.Elaborated,
                acquisitionEntryH.Observations,
                acquisitionEntryH.Status,
                acquisitionEntryH.IdAcReleaseH,
                acquisitionEntryH.IdAddressRelease,
                acquisitionEntryH.IdAddressEntry,
                acquisitionEntryH.IdAcCarrier,
                acquisitionEntryH.IdAcVehicle,
                acquisitionEntryH.IdDepartment
            });

            return insertedId;
        }

        public async Task<bool> AuthorizeEntryH(int id, int status)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionentriesh SET ");

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
