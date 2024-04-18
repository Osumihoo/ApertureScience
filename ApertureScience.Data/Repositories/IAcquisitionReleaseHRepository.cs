using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionReleaseHRepository
    {
        Task<IEnumerable<AcquisitionReleaseH>> GetAllReleaseH();
        Task<AcquisitionReleaseH> GetById(int id);
        Task<IEnumerable<AcquisitionReleaseH>> GetByDates(DateTime startDate, DateTime endDate);
        Task<int> InsertReleaseH(AcquisitionReleaseH acquisitionReleaseH);
        Task<bool> AuthorizeReleaseH(int id, int status);
    }
}
