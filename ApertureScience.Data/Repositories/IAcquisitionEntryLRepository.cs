using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionEntryLRepository
    {
        Task<IEnumerable<AcquisitionEntryL>> GetAllEntryL();
        Task<AcquisitionEntryL> GetById(int id);
        //Task<bool> InsertEntryL(AcquisitionEntryL acquisitionEntryL);
        Task<bool> InsertEntryL(IEnumerable<AcquisitionEntryL> acquisitionEntryList);
        Task<IEnumerable<AcquisitionEntryL>> GetAllEntryLByHeader(int id);
    }
}
