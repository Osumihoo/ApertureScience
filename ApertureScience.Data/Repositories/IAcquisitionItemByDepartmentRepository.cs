using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionItemByDepartmentRepository
    {
        Task<IEnumerable<AcquisitionItemByDepartment>> GetAllItemByDepartments();
        Task<AcquisitionItemByDepartment> GetByDepartment(int id);
        Task<bool> InsertItemByDepartment(AcquisitionItemByDepartment acquisitionItemByDepartment);
        Task<bool> UpdateItemByDepartment(AcquisitionItemByDepartment acquisitionItemByDepartment);
    }
}
