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
    public class AcquisitionTransferLRepository : IAcquisitionTransferLRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionTransferLRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<AcquisitionTransferL>> GetAllTransferL()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitiontransferl.Id, 
                                acquisitiontransferl.BaseLine, 
                                acquisitiontransferl.Code, 
                                acquisitiontransferl.Quantity,
                                acquisitiontransferl.Description,
                                acquisitiontransferl.IdAcTransferH,
                                acquisitiontransferl.IdAcSupplies
                        FROM acquisitiontransferl
                        INNER JOIN acquisitionsupplies ON acquisitionsupplies.Id = acquisitiontransferl.IdAcSupplies";

            return await db.QueryAsync<AcquisitionTransferL>(sql, new { });
        }

        public async Task<IEnumerable<AcquisitionTransferL>> GetTransferLByHeader(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitiontransferl.Id, 
                                acquisitiontransferl.BaseLine, 
                                acquisitiontransferl.Code, 
                                acquisitiontransferl.Quantity,
                                acquisitiontransferl.Description,
                                acquisitiontransferl.IdAcTransferH,
                                acquisitiontransferl.IdAcSupplies
                        FROM acquisitiontransferl
                        INNER JOIN acquisitionsupplies ON acquisitionsupplies.Id = acquisitiontransferl.IdAcSupplies
                        WHERE acquisitiontransferl.IdAcTransferH = @Id";

            return await db.QueryAsync<AcquisitionTransferL>(sql, new { Id = id });
        }

        public async Task<AcquisitionTransferL> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitiontransferl.Id, 
                                acquisitiontransferl.BaseLine, 
                                acquisitiontransferl.Code, 
                                acquisitiontransferl.Quantity,
                                acquisitiontransferl.Description,
                                acquisitiontransferl.IdAcTransferH,
                                acquisitiontransferl.IdAcSupplies
                        FROM acquisitiontransferl
                        INNER JOIN acquisitionsupplies ON acquisitionsupplies.Id = acquisitiontransferl.IdAcSupplies
                        WHERE acquisitiontransferl.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionTransferL>(sql, new { Id = id });
        }

        public async Task<bool> InsertTransferL(IEnumerable<AcquisitionTransferL> acquisitionTransferList)
        {
            var db = dbConnection();

            foreach (var acquisitionTransferL in acquisitionTransferList)
            {
                var sql = @"INSERT INTO acquisitionTransferl(   BaseLine,
                                                        Code,
                                                        Description,
                                                        Quantity,
                                                        IdAcTransferH,
                                                        IdAcSupplies
                                                    )
                    VALUES (@BaseLine,
                            @Code,
                            @Description,
                            @Quantity,
                            @IdAcTransferH,
                            @IdAcSupplies
                            )";

                var result = await db.ExecuteAsync(sql, acquisitionTransferL);

                if (result <= 0)
                {
                    return false; // Si ocurre un fallo en una de las inserciones, retornar falso.
                }
            }

            return true; // Todas las inserciones fueron exitosas.
        }
    }
}
