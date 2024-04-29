using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class AcquisitionTransferH
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Elaborated { get; set; }
        public string Observations { get; set; }
        public int? ReleaseCode { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReceptionCode { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public int Status { get; set; }
        public int IdAddressRelease { get; set; }
        public string AddressReleaseStreet { get; set; }
        public int IdAddressEntry { get; set; }
        public string AddressEntryStreet { get; set; }
        public int IdAcCarrier { get; set; }
        public string CarrierName { get; set; }
        public int IdAcVehicle { get; set; }
        public string VehicleName { get; set; }
        public int IdDepartmentRelease { get; set; }
        public string DepartmentReleaseName { get; set; }
        public int IdDepartmentEntry { get; set; }
        public string DepartmentEntryName { get; set; }

    }
}
