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
    public class AcquisitionEntryBySupplierLRepository : IAcquisitionEntryBySupplierLRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionEntryBySupplierLRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<AcquisitionEntryBySupplierL>> GetAllEntryBySupplierL()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriessuppliersl.Id, 
                                acquisitionentriessuppliersl.BaseLine,  
                                acquisitionentriessuppliersl.Quantity,
                                acquisitionentriessuppliersl.UnitPrice,
                                acquisitionentriessuppliersl.SubTotal,
                                acquisitionentriessuppliersl.IVA, 
                                acquisitionentriessuppliersl.Total, 
                                acquisitionentriessuppliersl.SupplierGuarantee,
                                acquisitionentriessuppliersl.BrandGuarantee,
                                acquisitionentriessuppliersl.Observations,
                                acquisitionentriessuppliersl.InvoiceSheets,
                                acquisitionentriessuppliersl.Contract,
                                acquisitionentriessuppliersl.IdAcEntrySuppliersH,
                                acquisitionentriessuppliersl.IdAcSupplies,
                                acquisitionsupplies.Code,
                                acquisitionsupplies.Description
                        FROM acquisitionentriessuppliersl
                        INNER JOIN 	acquisitionsupplies ON 	acquisitionsupplies.Id = acquisitionentriessuppliersl.IdAcSupplies";

            return await db.QueryAsync<AcquisitionEntryBySupplierL>(sql, new { });
        }

        public async Task<IEnumerable<AcquisitionEntryBySupplierL>> GetAllEntryBySupplierLByHeader(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriessuppliersl.Id, 
                                acquisitionentriessuppliersl.BaseLine,  
                                acquisitionentriessuppliersl.Quantity,
                                acquisitionentriessuppliersl.UnitPrice,
                                acquisitionentriessuppliersl.SubTotal,
                                acquisitionentriessuppliersl.IVA, 
                                acquisitionentriessuppliersl.Total, 
                                acquisitionentriessuppliersl.SupplierGuarantee,
                                acquisitionentriessuppliersl.BrandGuarantee,
                                acquisitionentriessuppliersl.Observations,
                                acquisitionentriessuppliersl.InvoiceSheets,
                                acquisitionentriessuppliersl.Contract,
                                acquisitionentriessuppliersl.IdAcEntrySuppliersH,
                                acquisitionentriessuppliersl.IdAcSupplies,
                                acquisitionsupplies.Code,
                                acquisitionsupplies.Description
                        FROM acquisitionentriessuppliersl
                        INNER JOIN 	acquisitionsupplies ON 	acquisitionsupplies.Id = acquisitionentriessuppliersl.IdAcSupplies
                        WHERE acquisitionentriessuppliersl.IdAcEntrySuppliersH = @Id";

            return await db.QueryAsync<AcquisitionEntryBySupplierL>(sql, new { Id = id });
        }

        public async Task<AcquisitionEntryBySupplierL> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriessuppliersl.Id, 
                                acquisitionentriessuppliersl.BaseLine,  
                                acquisitionentriessuppliersl.Quantity,
                                acquisitionentriessuppliersl.UnitPrice,
                                acquisitionentriessuppliersl.SubTotal,
                                acquisitionentriessuppliersl.IVA, 
                                acquisitionentriessuppliersl.Total, 
                                acquisitionentriessuppliersl.SupplierGuarantee,
                                acquisitionentriessuppliersl.BrandGuarantee,
                                acquisitionentriessuppliersl.Observations,
                                acquisitionentriessuppliersl.InvoiceSheets,
                                acquisitionentriessuppliersl.Contract,
                                acquisitionentriessuppliersl.IdAcEntrySuppliersH,
                                acquisitionentriessuppliersl.IdAcSupplies,
                                acquisitionsupplies.Code,
                                acquisitionsupplies.Description
                        FROM acquisitionentriessuppliersl
                        INNER JOIN 	acquisitionsupplies ON 	acquisitionsupplies.Id = acquisitionentriessuppliersl.IdAcSupplies
                        WHERE acquisitionentriessuppliersl.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionEntryBySupplierL>(sql, new { Id = id });
        }

        public async Task<bool> InsertEntryBySupplierL(IEnumerable<AcquisitionEntryBySupplierL> acquisitionEntryList)
        {
            foreach (var acquisitionReleaseL in acquisitionEntryList)
            {
                var db = dbConnection();

                var sql = @" INSERT INTO acquisitionentriessuppliersl(   BaseLine, 
                                                                        Quantity,
                                                                        UnitPrice,
                                                                        SubTotal,
                                                                        IVA, 
                                                                        Total, 
                                                                        SupplierGuarantee,
                                                                        BrandGuarantee,
                                                                        Observations,
                                                                        InvoiceSheets,
                                                                        Contract,
                                                                        IdAcEntrySuppliersH,
                                                                        IdAcSupplies
                                                                    )
                                    VALUES (@BaseLine,                                    
                                            @Quantity, 
                                            @UnitPrice,
                                            @SubTotal,
                                            @IVA, 
                                            @Total, 
                                            @SupplierGuarantee,
                                            @BrandGuarantee,
                                            @Observations,
                                            @InvoiceSheets,
                                            @Contract,
                                            @IdAcEntrySuppliersH,
                                            @IdAcSupplies
                                            )";

                var result = await db.ExecuteAsync(sql, acquisitionReleaseL);

                if (result <= 0)
                {
                    return false; // Si ocurre un fallo en una de las inserciones, retornar falso.
                }
            }

            return true;
        }
    }
}
