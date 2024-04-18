using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionSupplyRepository
    {
        Task<IEnumerable<AcquisitionSupply>> GetAllSupplies();
        Task<AcquisitionSupply> GetById(int id);
        Task<ResponseSupplyCode> GetTheLast();
        Task<bool> InsertSupply(AcquisitionSupply supply);
        Task<bool> UpdateSupply(AcquisitionSupply supply);
        Task<bool> OutSupply(IEnumerable<AcquisitionSupplyNewQuantity> acquisitionSupplyNewQuantity);
        Task<bool> InSupply(IEnumerable<AcquisitionSupplyNewQuantity> acquisitionSupplyNewQuantity);
        Task<bool> DeleteSupply(AcquisitionSupply supply);
    }
}
