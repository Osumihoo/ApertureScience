using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class Historical
    {
        public int Id { get; set; }
        public string MovementType { get; set; }
        public int IdUser { get; set; }
        public int IdFixedAsset { get; set; }
        public int IdArea { get; set; }
    }
}
