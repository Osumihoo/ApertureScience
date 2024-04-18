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
    public class AcquisitionEntryLRepository : IAcquisitionEntryLRepository
    {
        private readonly MySQLConfiguration _connectionString;

        public AcquisitionEntryLRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        public async Task<IEnumerable<AcquisitionEntryL>> GetAllEntryL()
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriesl.Id, 
                                acquisitionentriesl.BaseLine, 
                                acquisitionsupplies.Code, 
                                acquisitionentriesl.Quantity,
                                acquisitionsupplies.Description, 
                                acquisitionentriesl.UnitPrice,
                                acquisitionentriesl.SubTotal,
                                acquisitionentriesl.IVA, 
                                acquisitionentriesl.Total, 
                                acquisitionentriesl.IdAcEntryH,
                                acquisitionentriesl.IdAcSupplies
                        FROM acquisitionentriesl
                        INNER JOIN acquisitionsupplies ON acquisitionsupplies.Id = acquisitionentriesl.IdAcSupplies";

            return await db.QueryAsync<AcquisitionEntryL>(sql, new { });
        }

        public async Task<AcquisitionEntryL> GetById(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriesl.Id, 
                                acquisitionentriesl.BaseLine, 
                                acquisitionentriesl.Code, 
                                acquisitionentriesl.Quantity,
                                acquisitionentriesl.Description, 
                                acquisitionentriesl.UnitPrice,
                                acquisitionentriesl.SubTotal,
                                acquisitionentriesl.IVA, 
                                acquisitionentriesl.Total, 
                                acquisitionentriesl.IdAcEntryH
                        FROM acquisitionentriesl
                        WHERE acquisitionentriesl.Id = @Id";

            return await db.QueryFirstOrDefaultAsync<AcquisitionEntryL>(sql, new { Id = id });
        }

        public async Task<bool> InsertEntryL(IEnumerable<AcquisitionEntryL> acquisitionEntryList)
        {
            foreach (var acquisitionReleaseL in acquisitionEntryList)
            {
                var db = dbConnection();

                var sql = @" INSERT INTO acquisitionentriesl(   BaseLine, 
                                                                Code,
                                                                Description,
                                                                Quantity,
                                                                UnitPrice,
                                                                SubTotal,
                                                                IVA, 
                                                                Total, 
                                                                IdAcEntryH,
                                                                IdAcSupplies
                                                            )
                            VALUES (@BaseLine,                                    
                                    @Code, 
                                    @Description, 
                                    @Quantity, 
                                    @UnitPrice,
                                    @SubTotal,
                                    @IVA, 
                                    @Total, 
                                    @IdAcEntryH,
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

        public async Task<IEnumerable<AcquisitionEntryL>> GetAllEntryLByHeader(int id)
        {
            var db = dbConnection();

            var sql = @" SELECT acquisitionentriesl.Id, 
                                acquisitionentriesl.BaseLine, 
                                acquisitionentriesl.Code, 
                                acquisitionentriesl.Quantity,
                                acquisitionentriesl.Description, 
                                acquisitionentriesl.UnitPrice,
                                acquisitionentriesl.SubTotal,
                                acquisitionentriesl.IVA, 
                                acquisitionentriesl.Total, 
                                acquisitionentriesl.IdAcEntryH
                        FROM acquisitionentriesl
                        WHERE acquisitionentriesl.IdAcEntryH = @Id";

            return await db.QueryAsync<AcquisitionEntryL>(sql, new { Id = id });
        }
    }
}
