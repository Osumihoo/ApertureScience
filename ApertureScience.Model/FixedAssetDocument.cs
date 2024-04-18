using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class FixedAssetDocument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Favor de no ponerle el mismo nombre a tus variables aunque se encuentren en distinto entorno
        public IFormFile? FixedAssetImage { get; set; }
        public IFormFile? FixedAssetContract { get; set; }
        public IFormFile? FixedAssetWarranty { get; set; }
        //public IFormFile FixedAssetImg { get; set; }
        //public int Type { get; set; }
    }
}
