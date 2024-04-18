using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionEntryHRepository
    {
        Task<IEnumerable<AcquisitionEntryH>> GetAllEntryH();
        Task<AcquisitionEntryH> GetById(int id);
        Task<IEnumerable<AcquisitionEntryH>> GetByDates(DateTime startDate, DateTime endDate);
        Task<int> InsertEntryH(AcquisitionEntryH acquisitionEntryH);
        Task<bool> AuthorizeEntryH(int id, int status);
    }
}
