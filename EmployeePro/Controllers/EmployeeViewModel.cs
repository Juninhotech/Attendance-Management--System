using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePro.Controllers
{
    public class EmployeeViewModel
    {

        public string Password { get; set; }
        public int Id { get; set; }
        public string StaffID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string PunchIn { get; set; }
        public string PunchOut { get; set; }
    }
}
