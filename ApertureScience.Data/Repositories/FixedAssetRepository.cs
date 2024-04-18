using ApertureScience.Model;
using Dapper;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApertureScience.Data.Repositories
{
    public class FixedAssetRepository : IFixedAssetRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public FixedAssetRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<bool> DeleteFixedAsset(FixedAsset fixedAsset)
        {
            var db = dbConnection();

            var sql = @" UPDATE fixedassets
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { fixedAsset.Status, fixedAsset.Id });

            return result > 0;
        }

        public async Task<IEnumerable<FixedAsset>> GetAllFixedAsset()
        {
            var db = dbConnection();

            var sql = @" SELECT fixedassets.Id, 
                                fixedassets.Name, 
                                fixedassets.YearsUsefulLife, 
                                fixedassets.Status, 
                                fixedassets.AcquisitionDate,
                                fixedassets.ApplicationDate,
                                fixedassets.Invoice, 
                                fixedassets.Description, 
                                fixedassets.Amount, 
                                fixedassets.ShipmentCost, 
                                fixedassets.Discount,
                                fixedassets.SubTotal,
                                fixedassets.IEPS,
                                fixedassets.RetentionISR,
                                fixedassets.RetentionIVA,
                                fixedassets.IVA,
                                fixedassets.AditionalCost,
                                fixedassets.DepressionPercentage,
                                fixedassets.Total,
                                fixedassets.WarrantyDays,
                                fixedassets.MaintenanceCost,
                                fixedassets.IdSupplier,
                                suppliers.Businessname As NameSupplier,
                                fixedassets.IdCategory,
                                categorys.Name AS NameCategory,
                                fixedassets.IdDepartments,
                                departments.Name AS NameDepartments,
                                fixedassets.IdBusinessName,
                                businessname.Name AS NameBusinessName,
                                fixedassets.AssetClass,
                                fixedassets.FixedSAP,
                                fixedassets.Comments,
                                fixedassets.ContractRute,
                                fixedassets.WarrantyRute,
                                fixedassets.ImageRute
                            FROM fixedassets
                            INNER JOIN suppliers ON suppliers.Id = fixedassets.IdSupplier
                            INNER JOIN categorys ON categorys.Id = fixedassets.IdCategory
                            INNER JOIN departments ON departments.Id = fixedassets.IdDepartments
                            INNER JOIN businessname ON businessname.Id = fixedassets.IdBusinessName";

            return await db.QueryAsync<FixedAsset>(sql, new { });
        }

        public async Task<FixedAsset> GetByName(string name)
        {
            var db = dbConnection();

            var sql = @" SELECT fixedassets.Id, 
                                fixedassets.Name, 
                                fixedassets.YearsUsefulLife, 
                                fixedassets.Status, 
                                fixedassets.AcquisitionDate,
                                fixedassets.ApplicationDate,
                                fixedassets.Invoice, 
                                fixedassets.Description, 
                                fixedassets.Amount, 
                                fixedassets.ShipmentCost, 
                                fixedassets.Discount,
                                fixedassets.SubTotal,
                                fixedassets.IEPS,
                                fixedassets.RetentionISR,
                                fixedassets.RetentionIVA,
                                fixedassets.IVA,
                                fixedassets.AditionalCost,
                                fixedassets.DepressionPercentage,
                                fixedassets.Total,
                                fixedassets.WarrantyDays,
                                fixedassets.MaintenanceCost,
                                fixedassets.IdSupplier,
                                suppliers.Businessname As NameSupplier,
                                fixedassets.IdCategory,
                                categorys.Name AS NameCategory,
                                fixedassets.IdDepartments,
                                departments.Name AS NameDepartments,
                                fixedassets.IdBusinessName,
                                businessname.Name AS NameBusinessName,
                                fixedassets.AssetClass,
                                fixedassets.FixedSAP,
                                fixedassets.Comments,
                                fixedassets.ContractRute,
                                fixedassets.WarrantyRute,
                                fixedassets.ImageRute
                            FROM fixedassets
                            INNER JOIN suppliers ON suppliers.Id = fixedassets.IdSupplier
                            INNER JOIN categorys ON categorys.Id = fixedassets.IdCategory
                            INNER JOIN departments ON departments.Id = fixedassets.IdDepartments
                            INNER JOIN businessname ON businessname.Id = fixedassets.IdBusinessName
                            WHERE fixedassets.Name = @Name";

            return await db.QueryFirstOrDefaultAsync<FixedAsset>(sql, new { Name = name });
        }

        public async Task<int> InsertFixedAsset(FixedAsset fixedAsset)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO fixedassets(Name,
                                                YearsUsefulLife, 
                                                Status, 
                                                AcquisitionDate, 
                                                ApplicationDate, 
                                                Invoice, 
                                                Description, 
                                                Amount, 
                                                ShipmentCost, 
                                                Discount,
                                                SubTotal,
                                                IEPS,
                                                RetentionISR,
                                                RetentionIVA,
                                                IVA,
                                                AditionalCost,
                                                DepressionPercentage,
                                                Total,
                                                WarrantyDays,
                                                MaintenanceCost,
                                                IdSupplier,
                                                IdCategory,
                                                IdDepartments,
                                                IdBusinessName,
                                                AssetClass,
                                                FixedSAP,
                                                Comments)
                        VALUES (@Name,
                                @YearsUsefulLife, 
                                @Status, 
                                @AcquisitionDate, 
                                @ApplicationDate, 
                                @Invoice, 
                                @Description, 
                                @Amount, 
                                @ShipmentCost, 
                                @Discount,
                                @SubTotal,
                                @IEPS,
                                @RetentionISR,
                                @RetentionIVA,
                                @IVA,
                                @AditionalCost,
                                @DepressionPercentage,
                                @Total,
                                @WarrantyDays,
                                @MaintenanceCost,
                                @IdSupplier,
                                @IdCategory,
                                @IdDepartments,
                                @IdBusinessName,
                                @AssetClass,
                                @FixedSAP,
                                @Comments);
                        SELECT LAST_INSERT_ID();";

            //var result = await db.ExecuteAsync(sql, new
            var result = await db.ExecuteScalarAsync<int>(sql, new
            {   fixedAsset.Name,
                fixedAsset.YearsUsefulLife,
                fixedAsset.Status,
                fixedAsset.AcquisitionDate,
                fixedAsset.ApplicationDate,
                fixedAsset.Invoice,
                fixedAsset.Description,
                fixedAsset.Amount,
                fixedAsset.ShipmentCost,
                fixedAsset.Discount,
                fixedAsset.SubTotal,
                fixedAsset.IEPS,
                fixedAsset.RetentionISR,
                fixedAsset.RetentionIVA,
                fixedAsset.IVA,
                fixedAsset.AditionalCost,
                fixedAsset.DepressionPercentage,
                fixedAsset.Total,
                fixedAsset.WarrantyDays,
                fixedAsset.MaintenanceCost,
                fixedAsset.IdSupplier,
                fixedAsset.IdCategory,
                fixedAsset.IdDepartments,
                fixedAsset.IdBusinessName,
                fixedAsset.AssetClass,
                fixedAsset.FixedSAP,
                fixedAsset.Comments
            });

            return result;
        }

        public async Task<bool> UpdateFixedAsset(FixedAsset fixedAsset)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE fixedassets SET ");

            if (!string.IsNullOrWhiteSpace(fixedAsset.Name))
                sqlBuilder.Append("Name = @Name,");

            if (fixedAsset.YearsUsefulLife != 0)
                sqlBuilder.Append("YearsUsefulLife = @YearsUsefulLife,");

            if (fixedAsset.Status != null)
                sqlBuilder.Append("Status = @Status,");
            
            if (fixedAsset.AcquisitionDate != null)
                sqlBuilder.Append("AcquisitionDate = @AcquisitionDate,");

            if (fixedAsset.ApplicationDate != null)
                sqlBuilder.Append("ApplicationDate = @ApplicationDate,");

            if (fixedAsset.Invoice != null && fixedAsset.Invoice != 0)
                sqlBuilder.Append("Invoice = @Invoice,");

            if (!string.IsNullOrWhiteSpace(fixedAsset.Description))
                sqlBuilder.Append("Description = @Description,");

            if (fixedAsset.Amount != null && fixedAsset.Amount != 0)
                sqlBuilder.Append("Amount = @Amount,");

            if (fixedAsset.ShipmentCost != null && fixedAsset.ShipmentCost != 0)
                sqlBuilder.Append("ShipmentCost = @ShipmentCost,");

            if (fixedAsset.Discount != null && fixedAsset.Discount != 0)
                sqlBuilder.Append("Discount = @Discount,");

            if (fixedAsset.SubTotal != null && fixedAsset.SubTotal != 0)
                sqlBuilder.Append("SubTotal = @SubTotal,");

            if (fixedAsset.IEPS != null && fixedAsset.IEPS != 0)
                sqlBuilder.Append("IEPS = @IEPS,");

            if (fixedAsset.RetentionISR != null && fixedAsset.RetentionISR != 0)
                sqlBuilder.Append("RetentionISR = @RetentionISR,");

            if (fixedAsset.RetentionIVA != null && fixedAsset.RetentionIVA != 0)
                sqlBuilder.Append("RetentionIVA = @RetentionIVA,");

            if (fixedAsset.IVA != null && fixedAsset.IVA != 0)
                sqlBuilder.Append("IVA = @IVA,");

            if (fixedAsset.AditionalCost != null && fixedAsset.AditionalCost != 0)
                sqlBuilder.Append("AditionalCost = @AditionalCost,");

            if (fixedAsset.DepressionPercentage != null && fixedAsset.DepressionPercentage != 0)
                sqlBuilder.Append("DepressionPercentage = @DepressionPercentage,");

            if (fixedAsset.Total != null && fixedAsset.Total != 0)
                sqlBuilder.Append("Total = @Total,");

            if (fixedAsset.WarrantyDays != null && fixedAsset.WarrantyDays != 0)
                sqlBuilder.Append("WarrantyDays = @WarrantyDays,");

            if (fixedAsset.MaintenanceCost != null && fixedAsset.MaintenanceCost != 0)
                sqlBuilder.Append("MaintenanceCost = @MaintenanceCost,");

            if (!string.IsNullOrWhiteSpace(fixedAsset.AssetClass))
                sqlBuilder.Append("AssetClass = @AssetClass,");

            if (fixedAsset.FixedSAP != null && fixedAsset.FixedSAP != 0)
                sqlBuilder.Append("FixedSAP = @FixedSAP,");

            if (!string.IsNullOrWhiteSpace(fixedAsset.Comments))
                sqlBuilder.Append("Comments = @Comments,");

            if (fixedAsset.IdSupplier != null && fixedAsset.IdSupplier != 0)
                sqlBuilder.Append("IdSupplier = @IdSupplier,");

            if (fixedAsset.IdCategory != null && fixedAsset.IdCategory != 0)
                sqlBuilder.Append("IdCategory = @IdCategory,");

            if (fixedAsset.IdDepartments != null && fixedAsset.IdDepartments != 0)
                sqlBuilder.Append("IdDepartments = @IdDepartments,");

            if (fixedAsset.IdBusinessName != null && fixedAsset.IdBusinessName != 0)
                sqlBuilder.Append("IdBusinessName = @IdBusinessName,");
            

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                fixedAsset.Name,
                fixedAsset.YearsUsefulLife,
                fixedAsset.Status,
                fixedAsset.AcquisitionDate,
                fixedAsset.ApplicationDate,
                fixedAsset.Invoice,
                fixedAsset.Description,
                fixedAsset.Amount,
                fixedAsset.ShipmentCost,
                fixedAsset.Discount,
                fixedAsset.SubTotal,
                fixedAsset.IEPS,
                fixedAsset.RetentionISR,
                fixedAsset.RetentionIVA,
                fixedAsset.IVA,
                fixedAsset.AditionalCost,
                fixedAsset.DepressionPercentage,
                fixedAsset.Total,
                fixedAsset.WarrantyDays,
                fixedAsset.MaintenanceCost,
                fixedAsset.AssetClass,
                fixedAsset.FixedSAP,
                fixedAsset.Comments,
                fixedAsset.IdSupplier,
                fixedAsset.IdCategory,
                fixedAsset.IdDepartments,
                fixedAsset.IdBusinessName,
                fixedAsset.Id
            });

            return result > 0;
        }

        public async Task<ResponseFixedAssetName> GetTheLast()
        {
            var db = dbConnection();

            var sql = @" SELECT Name FROM fixedassets ORDER BY id DESC LIMIT 1;";

            return await db.QueryFirstOrDefaultAsync<ResponseFixedAssetName>(sql, new { });
        }

        public async Task<bool> InsertFixedAssetDocument(Dictionary<int, string> documentRute, int id)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE fixedassets SET ");

            foreach (var kvp in documentRute)
            {
                switch (kvp.Key)
                {
                    case 1:
                        sqlBuilder.Append("ImageRute = @ImageRute,");
                        break;
                    case 2:
                        sqlBuilder.Append("ContractRute = @ContractRute,");
                        break;
                    case 3:
                        sqlBuilder.Append("WarrantyRute = @WarrantyRute,");
                        break;
                        // Agrega más casos según sea necesario para otros tipos de documentos
                }
            }

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                ImageRute = documentRute.ContainsKey(1) ? documentRute[1] : null,
                ContractRute = documentRute.ContainsKey(2) ? documentRute[2] : null,
                WarrantyRute = documentRute.ContainsKey(3) ? documentRute[3] : null,
                // Asegúrate de agregar aquí más parámetros según sea necesario para otros tipos de documentos
                id
            });

            return result > 0;

        }
    }
}
