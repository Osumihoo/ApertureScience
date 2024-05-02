using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionTransferLRepository
    {
        Task<IEnumerable<AcquisitionTransferL>> GetAllTransferL();
        Task<AcquisitionTransferL> GetById(int id);
        Task<bool> InsertTransferL(IEnumerable<AcquisitionTransferL> acquisitionTransferList);
        Task<IEnumerable<AcquisitionTransferL>> GetTransferLByHeader(int id);

    }
}
