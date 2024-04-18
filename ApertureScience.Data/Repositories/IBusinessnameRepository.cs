using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IBusinessnameRepository
    {
        Task<IEnumerable<Businessname>> GetAllBusinessname();
        Task<Businessname> GetById(int id);
        Task<bool> InsertBusinessname(Businessname businessname);
        Task<bool> UpdateBusinessname(Businessname businessname);
        Task<bool> DeleteBusinessname(Businessname businessname);
    }
}
