using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionReleaseLRepository
    {
        Task<IEnumerable<AcquisitionReleaseL>> GetAllReleaseL();
        Task<AcquisitionReleaseL> GetById(int id);
        //Task<bool> InsertReleaseL(AcquisitionReleaseL acquisitionReleaseL);
        Task<bool> InsertReleaseL(IEnumerable<AcquisitionReleaseL> acquisitionReleaseList);
        Task<IEnumerable<AcquisitionReleaseL>> GetAllReleaseLByHeader(int id);
    }
}
