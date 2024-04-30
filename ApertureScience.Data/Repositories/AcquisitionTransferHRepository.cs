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
    public class AcquisitionTransferHRepository : IAcquisitionTransferHRepository
    {

        private readonly MySQLConfiguration _connectionString;

        public AcquisitionTransferHRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> AuthorizeTransferEntryH(int id, int status)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitiontransferh SET ");

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

        public async Task<bool> AuthorizeTransferReleaseH(int id, int status)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitiontransferh SET ");

            if (status == 2)
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

        public async Task<IEnumerable<AcquisitionTransferH>> GetAllTransferH()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitiontransferh.Id, 
                                acquisitiontransferh.Date, 
                                acquisitiontransferh.Type,
                                acquisitiontransferh.Elaborated,
                                acquisitiontransferh.Observations,
                                acquisitiontransferh.Status,
                                acquisitiontransferh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitiontransferh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitiontransferh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitiontransferh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitiontransferh.IdDepartmentRelease, 
                                departmentRelease.Name AS DepartmentReleaseName,
                                acquisitiontransferh.IdDepartmentEntry, 
                                departmentEntry.Name AS DepartmentEntryName
                        FROM acquisitiontransferh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitiontransferh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitiontransferh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitiontransferh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitiontransferh.IdAcVehicle
                        INNER JOIN departments AS departmentRelease ON departments.Id = acquisitiontransferh.IdDepartmentRelease
                        INNER JOIN departments AS departmentEntry ON departments.Id = acquisitiontransferh.IdDepartmentRelease
                        ORDER BY acquisitiontransferh.Id";

            return await db.QueryAsync<AcquisitionTransferH>(sql, new { });
        }

        public async Task<IEnumerable<AcquisitionTransferH>> GetByDates(DateTime startDate, DateTime endDate)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitiontransferh.Id, 
                                acquisitiontransferh.Date, 
                                acquisitiontransferh.Type,
                                acquisitiontransferh.Elaborated,
                                acquisitiontransferh.Observations,
                                acquisitiontransferh.Status,
                                acquisitiontransferh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitiontransferh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitiontransferh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitiontransferh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitiontransferh.IdDepartmentRelease, 
                                departmentRelease.Name AS DepartmentReleaseName,
                                acquisitiontransferh.IdDepartmentEntry, 
                                departmentEntry.Name AS DepartmentEntryName
                        FROM acquisitiontransferh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitiontransferh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitiontransferh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitiontransferh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitiontransferh.IdAcVehicle
                        INNER JOIN departments AS departmentRelease ON departments.Id = acquisitiontransferh.IdDepartmentRelease
                        INNER JOIN departments AS departmentEntry ON departments.Id = acquisitiontransferh.IdDepartmentRelease
                        WHERE acquisitiontransferh.Date >= @StartDate AND 
                                acquisitiontransferh.Date <= @EndDate
                        ORDER BY acquisitiontransferh.Id";

            return await db.QueryAsync<AcquisitionTransferH>(sql, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<AcquisitionTransferH> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitiontransferh.Id, 
                                acquisitiontransferh.Date, 
                                acquisitiontransferh.Type,
                                acquisitiontransferh.Elaborated,
                                acquisitiontransferh.Observations,
                                acquisitiontransferh.Status,
                                acquisitiontransferh.IdAddressRelease,
                                addressesRelease.Street AS AddressReleaseStreet,
                                acquisitiontransferh.IdAddressEntry,
                                addressesEntry.Street AS AddressEntryStreet,
                                acquisitiontransferh.IdAcCarrier, 
                                acquisitioncarriers.Name AS AcCarrierName,
                                acquisitiontransferh.IdAcVehicle, 
                                acquisitionvehicles.Name AS AcVehiclesName,
                                acquisitiontransferh.IdDepartmentRelease, 
                                departmentRelease.Name AS DepartmentReleaseName,
                                acquisitiontransferh.IdDepartmentEntry, 
                                departmentEntry.Name AS DepartmentEntryName
                        FROM acquisitiontransferh
                        INNER JOIN addresses AS addressesRelease ON addressesRelease.Id = acquisitiontransferh.IdAddressRelease
                        INNER JOIN addresses AS addressesEntry ON addressesEntry.Id = acquisitiontransferh.IdAddressEntry
                        INNER JOIN acquisitioncarriers ON acquisitioncarriers.Id = acquisitiontransferh.IdAcCarrier
                        INNER JOIN acquisitionvehicles ON acquisitionvehicles.Id = acquisitiontransferh.IdAcVehicle
                        INNER JOIN departments AS departmentRelease ON departments.Id = acquisitiontransferh.IdDepartmentRelease
                        INNER JOIN departments AS departmentEntry ON departments.Id = acquisitiontransferh.IdDepartmentRelease
                        WHERE acquisitiontransferh.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionTransferH>(sql, new { Id = id });
        }

        public async Task<int> InsertTransferH(AcquisitionTransferH acquisitionTransferH)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitiontransferh(  Date, 
                                                            Type,
                                                            Elaborated,
                                                            Observations,
                                                            Status,
                                                            IdAddressRelease,
                                                            IdAddressEntry,
                                                            IdAcCarrier,
                                                            IdAcVehicle,
                                                            IdDepartmentRelease,
                                                            IdDepartmentEntry)
                        VALUES (                @Date, 
                                                @Type,
                                                @Elaborated,
                                                @Observations,
                                                @Status,
                                                @IdAddressRelease,
                                                @IdAddressEntry,
                                                @IdAcCarrier,
                                                @IdAcVehicle,
                                                @IdDepartmentRelease,
                                                @IdDepartmentEntry);
                        SELECT LAST_INSERT_ID();";

            var insertedId = await db.ExecuteScalarAsync<int>(sql, new
            {
                acquisitionTransferH.Date,
                acquisitionTransferH.Type,
                acquisitionTransferH.Elaborated,
                acquisitionTransferH.Observations,
                acquisitionTransferH.Status,
                acquisitionTransferH.IdAddressRelease,
                acquisitionTransferH.IdAddressEntry,
                acquisitionTransferH.IdAcCarrier,
                acquisitionTransferH.IdAcVehicle,
                acquisitionTransferH.IdDepartmentRelease,
                acquisitionTransferH.IdDepartmentEntry
            });

            return insertedId;
        }
    }
}
