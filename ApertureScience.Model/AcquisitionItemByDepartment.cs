using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class AcquisitionItemByDepartment
    {
        public int Id { get; set; }
        public int OnHand { get; set; }
        public int Remaining { get; set; }
        public double Total { get; set; }
        public string CodeAcSupplies { get; set; }
        public int IdDepartment { get; set; }
        public string DepartmentName { get; set; }
    }
}
