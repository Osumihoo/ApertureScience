using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int IdUser { get; set; }
        public int IdFixedAsset {get; set; }
    }
}
