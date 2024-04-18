using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionCategoryRepository
    {
        Task<IEnumerable<AcquisitionCategory>> GetAllCategories();
        Task<AcquisitionCategory> GetById(int id);
        Task<bool> InsertCategory(AcquisitionCategory acquisitionCategory);
        Task<bool> UpdateCategory(AcquisitionCategory acquisitionCategory);
        Task<bool> DeleteCategory(AcquisitionCategory acquisitionCategory);
    }
}
