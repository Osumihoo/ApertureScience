using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class AcquisitionSupply
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public double ShippingCosts { get; set; }
        public double UnitCost { get; set; }
        public double Discount { get; set; }
        public double IEPS { get; set; }
        public double ISR { get; set; }
        public double IVA { get; set; }
        public double Total { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public int IdAcProduct { get; set; }
        public string AcProductName { get; set; }
        public int IdAcBrand { get; set; }
        public string AcBrandName { get; set; }
        public int IdAcModel { get; set; }
        public string AcModelName { get; set; }
        public int IdAcMeasurement { get; set; }
        public string AcUnitMeasurementName { get; set; }
        public int IdAcCategory { get; set; }
        public string AcCategoryName { get; set; }
    }
}
