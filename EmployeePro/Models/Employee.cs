using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePro.Models
{
    public class Employee
    {

        public int Id { get; set; }
        public string StaffID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public DateTime PunchIn { get; set; } = DateTime.UtcNow;
        public DateTime PunchOut { get; set; }
    }
}
