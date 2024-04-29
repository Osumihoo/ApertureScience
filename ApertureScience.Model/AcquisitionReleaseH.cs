using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class AcquisitionReleaseH
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Elaborated { get; set; }
        public string Observations { get; set; }
        public int? ReceptionCode { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public int Status { get; set; }
        public int IdAddressRelease { get; set; }
        public string AddressReleaseStreet { get; set; }
        public int IdAddressEntry { get; set; }
        public string AddressEntryStreet { get; set; }
        public int IdAcCarrier { get; set; }
        public string AcCarrierName { get; set; }
        public int IdAcVehicle { get; set; }
        public string AcVehiclesName { get; set; }
        public int IdDepartment { get; set; }
        public string DepartmentName { get; set; }
    }
}
