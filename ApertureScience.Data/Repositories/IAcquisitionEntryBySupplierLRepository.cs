using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionEntryBySupplierLRepository
    {
        Task<IEnumerable<AcquisitionEntryBySupplierL>> GetAllEntryBySupplierL();
        Task<AcquisitionEntryBySupplierL> GetById(int id);
        //Task<bool> InsertEntryL(AcquisitionEntryL acquisitionEntryL);
        Task<bool> InsertEntryBySupplierL(IEnumerable<AcquisitionEntryBySupplierL> acquisitionEntryList);
        Task<IEnumerable<AcquisitionEntryBySupplierL>> GetAllEntryBySupplierLByHeader(int id);
    }
}
