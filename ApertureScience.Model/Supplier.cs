using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Tradename { get; set; }
        public string Businessname { get; set; }
        public string Code { get; set; }
        public string RFC { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
    }
}
