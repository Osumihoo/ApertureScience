using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class AcquisitionEntryBySupplierH
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Elaborated { get; set; }
        public string Observations { get; set; }
        public int Status { get; set; }
        public int IdSuppliers { get; set; }
        public string Tradename { get; set; }
    }
}
