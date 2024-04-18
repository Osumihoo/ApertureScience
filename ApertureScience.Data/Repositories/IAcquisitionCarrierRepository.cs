using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionCarrierRepository
    {
        Task<IEnumerable<AcquisitionCarrier>> GetAllCarriers();
        Task<AcquisitionCarrier> GetById(int id);
        Task<bool> InsertCarrier(AcquisitionCarrier acquisitionCarrier);
        Task<bool> UpdateCarrier(AcquisitionCarrier acquisitionCarrier);
        Task<bool> DeleteCarrier(AcquisitionCarrier acquisitionCarrier);
    }
}
