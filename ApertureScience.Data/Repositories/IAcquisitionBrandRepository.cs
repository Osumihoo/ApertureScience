using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionBrandRepository
    {
        Task<IEnumerable<AcquisitionBrand>> GetAllBrands();
        Task<AcquisitionBrand> GetById(int id);
        Task<bool> InsertBrand(AcquisitionBrand acquisitionBrand);
        Task<bool> UpdateBrand(AcquisitionBrand acquisitionBrand);
        Task<bool> DeleteBrand(AcquisitionBrand acquisitionBrand);
    }
}
