using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class SAPItemDepreciationParameters
    {
        public string FiscalYear { get; set; } //= "2023";
        public string DepreciationArea { get; set; } //= "ACTIVOS FIJOS";
        public string DepreciationStartDate { get; set; } //= "2023-01-01T00:00:00Z";
        //public string DepreciationEndDate { get; set; } //= "2023-12-31T00:00:00Z";
        public string UsefulLife { get; set; } //= "12";
        public string RemainingLife { get; set; } //= "12.0";
        public string DepreciationType { get; set; } //= "50";
        public string TotalUnitsInUsefulLife { get; set; } //= "0";
        public string RemainingUnits { get; set; } //= "0";
        public string StandardUnits { get; set; } //= "0";

    }
}
