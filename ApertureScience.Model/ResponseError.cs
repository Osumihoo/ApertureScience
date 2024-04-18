using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class ResponseError
    {
        public int code {  get; set; }
        public string message { get; set; }
        public string[] error { get; set; }
    }
}
