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
    public class AcquisitionReleaseLRepository : IAcquisitionReleaseLRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionReleaseLRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<AcquisitionReleaseL>> GetAllReleaseL()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionreleasel.Id, 
                                acquisitionreleasel.BaseLine, 
                                acquisitionsupplies.Code, 
                                acquisitionreleasel.Quantity,
                                acquisitionsupplies.Description, 
                                acquisitionreleasel.UnitPrice,
                                acquisitionreleasel.Amount,
                                acquisitionreleasel.Discount,
                                acquisitionreleasel.ShippingCost,
                                acquisitionreleasel.SubTotal,
                                acquisitionreleasel.IEPS, 
                                acquisitionreleasel.ISR, 
                                acquisitionreleasel.IVA, 
                                acquisitionreleasel.Total, 
                                acquisitionreleasel.IdAcReleaseH,
                                acquisitionreleasel.IdAcSupplies
                        FROM acquisitionreleasel
                        INNER JOIN acquisitionsupplies ON acquisitionsupplies.Id = acquisitionreleasel.IdAcSupplies";

            return await db.QueryAsync<AcquisitionReleaseL>(sql, new { });
        }

        public async Task<AcquisitionReleaseL> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionreleasel.Id, 
                                acquisitionreleasel.BaseLine, 
                                acquisitionsupplies.Code, 
                                acquisitionreleasel.Quantity,
                                acquisitionsupplies.Description, 
                                acquisitionreleasel.UnitPrice,
                                acquisitionreleasel.Amount,
                                acquisitionreleasel.Discount,
                                acquisitionreleasel.ShippingCost,
                                acquisitionreleasel.SubTotal,
                                acquisitionreleasel.IEPS, 
                                acquisitionreleasel.ISR, 
                                acquisitionreleasel.IVA, 
                                acquisitionreleasel.Total, 
                                acquisitionreleasel.IdAcReleaseH,
                                acquisitionreleasel.IdAcSupplies
                        FROM acquisitionreleasel
                        INNER JOIN acquisitionsupplies ON acquisitionsupplies.Id = acquisitionreleasel.IdAcSupplies
                        WHERE acquisitionreleasel.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionReleaseL>(sql, new { Id = id });
        }

        public async Task<bool> InsertReleaseL(IEnumerable<AcquisitionReleaseL> acquisitionReleaseList)
        {
            var db = dbConnection();

            foreach (var acquisitionReleaseL in acquisitionReleaseList)
            {
                var sql = @"INSERT INTO acquisitionreleasel(   BaseLine,
                                                        Quantity,
                                                        UnitPrice,
                                                        Amount,
                                                        Discount,
                                                        ShippingCost,
                                                        SubTotal,
                                                        IEPS,
                                                        ISR,
                                                        IVA,
                                                        Total,
                                                        IdAcReleaseH,
                                                        IdAcSupplies
                                                    )
                    VALUES (@BaseLine,
                            @Quantity,
                            @UnitPrice,
                            @Amount,
                            @Discount,
                            @ShippingCost,
                            @SubTotal,
                            @IEPS,
                            @ISR,
                            @IVA,
                            @Total,
                            @IdAcReleaseH,
                            @IdAcSupplies
                            )";

                var result = await db.ExecuteAsync(sql, acquisitionReleaseL);

                if (result <= 0)
                {
                    return false; // Si ocurre un fallo en una de las inserciones, retornar falso.
                }
            }

            return true; // Todas las inserciones fueron exitosas.
        }

        public async Task<IEnumerable<AcquisitionReleaseL>> GetAllReleaseLByHeader(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionreleasel.Id, 
                                acquisitionreleasel.BaseLine, 
                                acquisitionsupplies.Code, 
                                acquisitionreleasel.Quantity,
                                acquisitionsupplies.Description, 
                                acquisitionreleasel.UnitPrice,
                                acquisitionreleasel.Amount,
                                acquisitionreleasel.Discount,
                                acquisitionreleasel.ShippingCost,
                                acquisitionreleasel.SubTotal,
                                acquisitionreleasel.IEPS, 
                                acquisitionreleasel.ISR, 
                                acquisitionreleasel.IVA, 
                                acquisitionreleasel.Total, 
                                acquisitionreleasel.IdAcReleaseH,
                                acquisitionreleasel.IdAcSupplies
                        FROM acquisitionreleasel
                        INNER JOIN acquisitionsupplies ON acquisitionsupplies.Id = acquisitionreleasel.IdAcSupplies
                        WHERE acquisitionreleasel.IdAcReleaseH = @Id";

            return await db.QueryAsync<AcquisitionReleaseL>(sql, new { Id = id });
        }
    }
}
