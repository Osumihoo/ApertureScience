using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionVehicleRepository
    {
        Task<IEnumerable<AcquisitionVehicle>> GetAllVehicles();
        Task<AcquisitionVehicle> GetById(int id);
        Task<bool> InsertVehicle(AcquisitionVehicle acquisitionVehicle);
        Task<bool> UpdateVehicle(AcquisitionVehicle acquisitionVehicle);
        Task<bool> DeleteVehicle(AcquisitionVehicle acquisitionVehicle);
    }
}
