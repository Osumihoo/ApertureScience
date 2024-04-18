using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAllAddresses();
        Task<Address> GetById(int id);
        Task<bool> InsertAddress(Address address);
        Task<bool> UpdateAddress(Address address);
        Task<bool> DeleteAddress(Address address);
    }
}
