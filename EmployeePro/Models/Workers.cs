using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePro.Models
{
    public class Workers
    {

        public int id { get; set; }
        public string StaffID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } = "";
        public string Department { get; set; }
    }
}
