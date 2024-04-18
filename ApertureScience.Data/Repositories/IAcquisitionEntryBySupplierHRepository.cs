using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionEntryBySupplierHRepository
    {
        Task<IEnumerable<AcquisitionEntryBySupplierH>> GetAllEntryBySupplierH();
        Task<AcquisitionEntryBySupplierH> GetById(int id);
        Task<IEnumerable<AcquisitionEntryBySupplierH>> GetByDates(DateTime startDate, DateTime endDate);
        Task<int> InsertEntryBySupplierH(AcquisitionEntryBySupplierH acquisitionEntryH);
    }
}
