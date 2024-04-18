using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class SAPItemDepreciationParametersPatch
    {
        public string FiscalYear { get; set; } //= "2023";
        public string DepreciationArea { get; set; } //= "ACTIVOS FIJOS";
        public string DepreciationStartDate { get; set; } //= "2023-01-01T00:00:00Z";
    }
}
