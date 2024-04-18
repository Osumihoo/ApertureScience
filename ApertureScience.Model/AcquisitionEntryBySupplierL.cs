using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class AcquisitionEntryBySupplierL
    {
        public int Id { get; set; }
        public int BaseLine { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public double SubTotal { get; set; }
        public double IVA { get; set; }
        public double Total { get; set; }
        public int SupplierGuarantee { get; set; }
        public int BrandGuarantee { get; set; }
        public string Observations { get; set; }
        public string InvoiceSheets { get; set; }
        public int Contract { get; set; }
        public int IdAcEntrySuppliersH { get; set; }
        public int IdAcSupplies{ get; set; }
    }
}
