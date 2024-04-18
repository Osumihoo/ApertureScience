using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetAllAreas();
        Task<Area> GetById(int id);
        Task<bool> InsertArea(Area category);
        Task<bool> UpdateArea(Area category);
        Task<bool> DeleteArea(Area category);
    }
}
