using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class SAPPurchaseInvoicesLines
    {
        public string ItemCode { get; set; }   //AFPR0008
        public int Quantity { get; set; }      //1
        public string TaxCode { get; set; }    //IVAC16
        public double UnitPrice { get; set; }  //50
    }
}
