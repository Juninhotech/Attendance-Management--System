using EmployeePro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePro.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> EmployeeDb { get; set; }
        public DbSet<Workers> WorkersDb { get; set; }
        public DbSet<Department> Departments { get; set; }
        public object Employee { get; internal set; }
    }
}
