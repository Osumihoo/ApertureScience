using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class SAPPurchaseInvoices
    {
        public string CardCode { get; set; } //P00030
        public string Comments { get; set; } //P00030

        public List<SAPPurchaseInvoicesLines> DocumentLines { get; set; }
    }
}
