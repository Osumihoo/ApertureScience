using ApertureScience.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Data.Repositories
{
    public interface IAcquisitionMeasurementRepository
    {
        Task<IEnumerable<AcquisitionMeasurement>> GetAllMeasurements();
        Task<AcquisitionMeasurement> GetById(int id);
        Task<bool> InsertMeasurement(AcquisitionMeasurement acquisitionMeasurement);
        Task<bool> UpdateMeasurement(AcquisitionMeasurement acquisitionMeasurement);
        Task<bool> DeleteMeasurement(AcquisitionMeasurement acquisitionMeasurement);
    }
}
