using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionModelRepository
    {
        Task<IEnumerable<AcquisitionModel>> GetAllModels();
        Task<AcquisitionModel> GetById(int id);
        Task<bool> InsertModel(AcquisitionModel acquisitionModel);
        Task<bool> UpdateModel(AcquisitionModel acquisitionModel);
        Task<bool> DeleteModel(AcquisitionModel acquisitionModel);
    }
}
