using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class ResponseLogin
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastNameP { get; set; } = string.Empty;
        public string LastNameM { get; set; } = string.Empty;
        public int IdRole { get; set; }
        public int IdDepartment { get; set; }
        public bool conflicts { get; set; }
        
    }


}
