using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace ApertureScience.Model
{
    public class FixedAsset
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public int YearsUsefulLife { get; set; }
        public int Status { get; set; }
        public string ContractRute { get; set; }
        public string WarrantyRute { get; set; }
        public string ImageRute { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int Invoice { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double ShipmentCost { get; set; }
        public double Discount { get; set;}
        public double SubTotal { get; set; }
        public double IEPS { get; set; }
        public double RetentionISR { get; set; }
        public double RetentionIVA { get; set;}
        public double IVA { get; set; }
        public double AditionalCost { get; set; }
        public int DepressionPercentage { get; set;}
        public double Total { get; set;}
        public int WarrantyDays { get; set; }
        public double MaintenanceCost { get; set; }
        public int IdSupplier { get; set;}
        public string NameSupplier { get; set; }
        public int IdCategory { get; set;}
        public string NameCategory { get; set; }
        public int IdDepartments { get; set;}
        public string NameDepartments { get; set; }
        public int IdBusinessName { get; set;}
        public string NameBusinessName { get; set; }
        public string AssetClass { get; set; }
        public int FixedSAP { get; set; }
        public string Comments { get; set; }

    }
}
