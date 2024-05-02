using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class AcquisitionTransferL
    {
        public int Id { get; set; }
        public int BaseLine { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int IdAcTransferH { get; set; }
        public int IdAcSupplies { get; set; }
    }
}
