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
    public class AcquisitionSupplyRepository : IAcquisitionSupplyRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionSupplyRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<bool> DeleteSupply(AcquisitionSupply supply)
        {
            var db = dbConnection();

            var sql = @" UPDATE acquisitionsupplies
                            SET Status = @Status
                            WHERE Id = @Id";

            var result = await db.ExecuteAsync(sql, new
            { supply.Status, supply.Id });

            return result > 0;
        }

        public async Task<IEnumerable<AcquisitionSupply>> GetAllSupplies()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionsupplies.Id, 
                                acquisitionsupplies.Code, 
                                acquisitionsupplies.Color,
                                acquisitionsupplies.Description,
                                acquisitionsupplies.ShippingCosts,
                                acquisitionsupplies.UnitCost,
                                acquisitionsupplies.Discount,
                                acquisitionsupplies.IEPS,
                                acquisitionsupplies.ISR,
                                acquisitionsupplies.IVA,
                                acquisitionsupplies.Total,
                                acquisitionsupplies.Quantity,
                                acquisitionsupplies.Status,
                                acquisitionsupplies.IdAcProduct,
                                acquisitionproducts.Name AS AcProductName,
                                acquisitionsupplies.IdAcBrand,
                                acquisitionbrands.Name AS AcBrandName,
                                acquisitionsupplies.IdAcModel,
                                acquisitionmodels.Name AS AcModelName,
                                acquisitionsupplies.IdAcMeasurement,
                                acquisitionmeasurements.Name AS AcUnitMeasurementName, 
                                acquisitionsupplies.IdAcCategory, 
                                acquisitioncategories.Name AS AcCategoryName 
                        FROM acquisitionsupplies
                        INNER JOIN acquisitionproducts ON acquisitionproducts.Id = acquisitionsupplies.IdAcProduct
                        INNER JOIN acquisitionbrands ON acquisitionbrands.Id = acquisitionsupplies.IdAcBrand
                        INNER JOIN acquisitionmodels ON acquisitionmodels.Id = acquisitionsupplies.IdAcModel
                        INNER JOIN acquisitionmeasurements ON acquisitionmeasurements.Id = acquisitionsupplies.IdAcMeasurement
                        INNER JOIN acquisitioncategories ON acquisitioncategories.Id = acquisitionsupplies.IdAcCategory";

            return await db.QueryAsync<AcquisitionSupply>(sql, new { });
        }

        public async Task<AcquisitionSupply> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionsupplies.Id, 
                                acquisitionsupplies.Code, 
                                acquisitionsupplies.Color,
                                acquisitionsupplies.Description,
                                acquisitionsupplies.ShippingCosts,
                                acquisitionsupplies.UnitCost,
                                acquisitionsupplies.Discount,
                                acquisitionsupplies.IEPS,
                                acquisitionsupplies.ISR,
                                acquisitionsupplies.IVA,
                                acquisitionsupplies.Total,
                                acquisitionsupplies.Quantity,
                                acquisitionsupplies.Status,
                                acquisitionsupplies.IdAcProduct,
                                acquisitionproducts.Name AS AcProductName,
                                acquisitionsupplies.IdAcBrand,
                                acquisitionbrands.Name AS AcBrandName,
                                acquisitionsupplies.IdAcModel,
                                acquisitionmodels.Name AS AcModelName,
                                acquisitionsupplies.IdAcMeasurement,
                                acquisitionmeasurements.Name AS AcUnitMeasurementName, 
                                acquisitionsupplies.IdAcCategory, 
                                acquisitioncategories.Name AS AcCategoryName 
                        FROM acquisitionsupplies
                        INNER JOIN acquisitionproducts ON acquisitionproducts.Id = acquisitionsupplies.IdAcProduct
                        INNER JOIN acquisitionbrands ON acquisitionbrands.Id = acquisitionsupplies.IdAcBrand
                        INNER JOIN acquisitionmodels ON acquisitionmodels.Id = acquisitionsupplies.IdAcModel
                        INNER JOIN acquisitionmeasurements ON acquisitionmeasurements.Id = acquisitionsupplies.IdAcMeasurement
                        INNER JOIN acquisitioncategories ON acquisitioncategories.Id = acquisitionsupplies.IdAcCategory
                        WHERE acquisitionsupplies.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionSupply>(sql, new { Id = id });
        }
        public async Task<ResponseSupplyCode> GetTheLast()
        {
            var db = dbConnection();

            var sql = @" SELECT Code FROM acquisitionsupplies 
                                    WHERE Id = (SELECT Max(Id) FROM acquisitionsupplies);";

            return await db.QueryFirstOrDefaultAsync<ResponseSupplyCode>(sql, new { });
        }

        public async Task<bool> InsertSupply(AcquisitionSupply supply)
        {
            var db = dbConnection();

            var sql = @" INSERT INTO acquisitionsupplies(  Code, 
                                                            Color,
                                                            Description,
                                                            ShippingCosts,
                                                            UnitCost,
                                                            Discount,
                                                            IEPS,
                                                            ISR,
                                                            IVA,
                                                            Total,
                                                            Quantity,
                                                            Status,
                                                            IdAcProduct,
                                                            IdAcBrand,
                                                            IdAcModel,
                                                            IdAcMeasurement,
                                                            IdAcCategory)
                        VALUES (                @Code, 
                                                @Color,
                                                @Description,
                                                @ShippingCosts,
                                                @UnitCost,
                                                @Discount,
                                                @IEPS,
                                                @ISR,
                                                @IVA,
                                                @Total,
                                                @Quantity,
                                                @Status,
                                                @IdAcProduct,
                                                @IdAcBrand,
                                                @IdAcModel,
                                                @IdAcMeasurement,
                                                @IdAcCategory)";

            var result = await db.ExecuteAsync(sql, new
            {   supply.Code, 
                supply.Color, 
                supply.Description,
                supply.ShippingCosts, 
                supply.UnitCost, 
                supply.Discount,
                supply.IEPS, 
                supply.ISR, 
                supply.IVA,
                supply.Total, 
                supply.Quantity, 
                supply.Status, 
                supply.IdAcProduct, 
                supply.IdAcBrand,
                supply.IdAcModel, 
                supply.IdAcMeasurement, 
                supply.IdAcCategory
            });

            return result > 0;
        }

        public async Task<bool> UpdateSupply(AcquisitionSupply supply)
        {
            var db = dbConnection();

            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("UPDATE acquisitionsupplies SET ");

            if (!string.IsNullOrWhiteSpace(supply.Code))
                sqlBuilder.Append("Code = @Code,");

            if (!string.IsNullOrWhiteSpace(supply.Color))
                sqlBuilder.Append("Color = @Color,");

            if (!string.IsNullOrWhiteSpace(supply.Description))
                sqlBuilder.Append("Description = @Description,");

            if (supply.ShippingCosts != null)
                sqlBuilder.Append("ShippingCosts = @ShippingCosts,");

            if (supply.UnitCost != null)
                sqlBuilder.Append("UnitCost = @UnitCost,");

            if (supply.Discount != null)
                sqlBuilder.Append("Discount = @Discount,");

            if (supply.IEPS != null)
                sqlBuilder.Append("IEPS = @IEPS,");

            if (supply.ISR != null)
                sqlBuilder.Append("ISR = @ISR,");

            if (supply.IVA != null)
                sqlBuilder.Append("IVA = @IVA,");

            if (supply.Total != null)
                sqlBuilder.Append("Total = @Total,");

            if (supply.Quantity != null)
                sqlBuilder.Append("Quantity = @Quantity,");

            if (supply.Status != null)
                sqlBuilder.Append("Status = @Status,");

            if (supply.IdAcProduct != null)
                sqlBuilder.Append("IdAcProduct = @IdAcProduct,");

            if (supply.IdAcBrand != null)
                sqlBuilder.Append("IdAcBrand = @IdAcBrand,");

            if (supply.IdAcModel != null)
                sqlBuilder.Append("IdAcModel = @IdAcModel,");

            if (supply.IdAcMeasurement != null)
                sqlBuilder.Append("IdAcMeasurement = @IdAcMeasurement,");

            if (supply.IdAcCategory != null)
                sqlBuilder.Append("IdAcCategory = @IdAcCategory,");

            // Agrega más condiciones según sea necesario

            // Elimina la última coma si hay campos en el SET
            if (sqlBuilder.ToString().EndsWith(","))
                sqlBuilder.Length--; // Elimina la última coma

            sqlBuilder.Append(" WHERE Id = @Id");

            var sql = sqlBuilder.ToString();

            var result = await db.ExecuteAsync(sql, new
            {
                supply.Code,
                supply.Color,
                supply.Description,
                supply.ShippingCosts,
                supply.UnitCost,
                supply.Discount,
                supply.IEPS,
                supply.ISR,
                supply.IVA,
                supply.Total,
                supply.Quantity,
                supply.Status,
                supply.IdAcProduct,
                supply.IdAcBrand,
                supply.IdAcModel,
                supply.IdAcMeasurement,
                supply.IdAcCategory, 
                supply.Id 
            });

            return result > 0;
        }

        public async Task<bool> OutSupply(IEnumerable<AcquisitionSupplyNewQuantity> acquisitionSupplyNewQuantityList)
        {
            foreach (var acquisitionSupplyNewQuantity in acquisitionSupplyNewQuantityList)
            {
                var db = dbConnection();

                var sql = @"
                            UPDATE acquisitionsupplies
                            SET Quantity = Quantity - @NewQuantity
                            WHERE Id = @SupplyId";

                var result = await db.ExecuteAsync(sql, acquisitionSupplyNewQuantity);

                if (result <= 0)
                {
                    return false; // Si ocurre un fallo en una de las inserciones, retornar falso.
                }
            }
            return true;
        }

        public async Task<bool> InSupply(IEnumerable<AcquisitionSupplyNewQuantity> acquisitionSupplyNewQuantityList)
        {
            foreach (var acquisitionSupplyNewQuantity in acquisitionSupplyNewQuantityList)
            {
                var db = dbConnection();

                var sql = @"
                            UPDATE acquisitionsupplies
                            SET Quantity = Quantity + @NewQuantity
                            WHERE Id = @SupplyId";

                var result = await db.ExecuteAsync(sql, acquisitionSupplyNewQuantity);

                if (result <= 0)
                {
                    return false; // Si ocurre un fallo en una de las inserciones, retornar falso.
                }
            }
            return true;
        }
    }
}
