using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastNameP { get; set; }
        public string LastNameM { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public int IdRole { get; set; }
        public string RolName { get; set; }
        public int IdDepartment { get; set; }
        public string DepartmentName { get; set; }
    }
}
