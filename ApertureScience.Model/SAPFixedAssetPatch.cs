using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class SAPFixedAssetPatch
    {
        public string ItemCode { get; set; } //= "AFPR0003";
        public List<SAPItemDepreciationParametersPatch> ItemDepreciationParameters { get; set; }
    }
}
