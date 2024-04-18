using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetAllRoles();
        Task<Rol> GetById(int id);
        Task<bool> InsertRol(Rol rol);
        Task<bool> UpdateRol(Rol rol);
        Task<bool> DeleteRol(Rol rol);
    }
}
