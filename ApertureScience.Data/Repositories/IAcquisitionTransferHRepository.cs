using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionTransferHRepository
    {
        Task<IEnumerable<AcquisitionTransferH>> GetAllTransferH();
        Task<AcquisitionTransferH> GetById(int id);
        Task<IEnumerable<AcquisitionTransferH>> GetByDates(DateTime startDate, DateTime endDate);
        Task<int> InsertTransferH(AcquisitionTransferH acquisitionTransferH);
        Task<bool> AuthorizeTransferEntryH(int id, int status);
        Task<bool> AuthorizeTransferReleaseH(int id, int status);
    }
}
