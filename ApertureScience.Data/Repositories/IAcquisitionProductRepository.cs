using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionProductRepository
    {
        Task<IEnumerable<AcquisitionProduct>> GetAllProducts();
        Task<AcquisitionProduct> GetById(int id);
        Task<bool> InsertProduct(AcquisitionProduct acquisitionProduct);
        Task<bool> UpdateProduct(AcquisitionProduct acquisitionProduct);
        Task<bool> DeleteProduct(AcquisitionProduct acquisitionProduct);
    }
}
