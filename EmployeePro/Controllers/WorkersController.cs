using EmployeePro.Data;
using EmployeePro.Models;
using EmployeePro.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : Controller
    {

        private readonly EmployeeDbContext _employeeDbContext;

        public WorkersController(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }


        public static string HashedPassword(string password)
        {
            SHA256 hash = SHA256.Create();
            var passwordBytes = Encoding.Default.GetBytes(password);
            var hashedpassword = hash.ComputeHash(passwordBytes);
            var hexString = BitConverter.ToString(hashedpassword);
            return hexString;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllWorkers()
        {
            var workers = await _employeeDbContext.WorkersDb.Select(x => new WorkersViewModel
            {
                id = x.id,
                StaffID = x.StaffID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Password = x.Password,
                Department = x.Department,
            }).ToListAsync();

            return Ok(workers);
        }

        [Route("getAllDepartment")]

        [HttpGet]

        public async Task<IActionResult> GetAllDepartment()
        {
            var department = await _employeeDbContext.Departments.Select(x => new Department
            {
               Id = x.Id,
               Position =x.Position
            }).ToListAsync();

            return Ok(department);
        }

        [Route("addNewDepartment")]

        [HttpPost]
        public async Task<IActionResult>AddDepartment([FromBody] Department adddept)
        {
            var dept = new Department
            {
                Position = adddept.Position
            };

            await _employeeDbContext.AddAsync(dept);
            await _employeeDbContext.SaveChangesAsync();

            return Ok("New department added successfully");
        }

        [HttpPost]
        public async Task<IActionResult> AddWorker([FromBody]WorkersViewModel postWorker)
        {
            ErrorViewModel rrr = new ErrorViewModel();
            try
            {
                var workers = new Workers
                {
                    StaffID = postWorker.StaffID,
                    FirstName = postWorker.FirstName,
                    LastName = postWorker.LastName,
                    Email = postWorker.Email,
                    Password = HashedPassword(postWorker.Password),
                    Department = postWorker.Department
                };

                var IsStaffIdExist = await _employeeDbContext.WorkersDb.Where(x => x.StaffID == postWorker.StaffID).AnyAsync();

                if (!IsStaffIdExist)
                {
                    rrr.code = "00";
                    rrr.fname = $"{workers.FirstName} {workers.LastName}";
                    rrr.Message = "You've successfully added";
                    await _employeeDbContext.WorkersDb.AddAsync(workers);
                    await _employeeDbContext.SaveChangesAsync();

                    return Json(rrr);
                }
                else
                {
                    rrr.code = "01";
                    rrr.Message = $"Staff with the ID {workers.StaffID} already exist";
                    return Json(rrr);
                }
            }
            catch (Exception)
            {
                rrr.Message = "An error occurs";
                rrr.code = "05";
                return Json(rrr);
            }
           

            

        }

        [HttpGet]
        [Route("{staffID}")]

        public async Task<IActionResult> GetAllWorkers([FromRoute] string staffID)
        {
            var workers = await _employeeDbContext.WorkersDb.FirstOrDefaultAsync(x => x.StaffID == staffID);

            if (workers == null)
            {
                return NotFound("Staff doesn't exist");
            }
            return Ok(workers);
        }

        [HttpPut]
        [Route("{staffID}")]
        public async Task<IActionResult> ModifyEmployee([FromRoute] string staffID, Workers modifyEmployee)
        {
            var workers = await _employeeDbContext.WorkersDb.Where(x => x.StaffID.Contains(staffID)).FirstOrDefaultAsync();

            if (workers == null)
            {
                return NotFound("Staff not found");
            }

            workers.StaffID = modifyEmployee.StaffID;
            workers.FirstName = modifyEmployee.FirstName;
            workers.LastName = modifyEmployee.LastName;
            workers.Email = modifyEmployee.Email;
            workers.Password = HashedPassword(modifyEmployee.Password);
            workers.Department = modifyEmployee.Department;
           _employeeDbContext.Entry(workers).State = EntityState.Modified;
            await _employeeDbContext.SaveChangesAsync();
            return Ok(workers);

        }

        

        [HttpDelete]
        [Route("{staffID}")]

        public async Task<IActionResult> DeleteWorker([FromRoute] string staffID)
        {
            var workers = await _employeeDbContext.WorkersDb.Where(x => x.StaffID.Contains(staffID)).FirstOrDefaultAsync();

            if (workers == null)
            {
                return NotFound("Can't delete staff that doesn't exist");
            }

            _employeeDbContext.WorkersDb.Remove(workers);
            await _employeeDbContext.SaveChangesAsync();

            return Ok($"Employee with staff number {staffID} was removed");
        }

  
    }
}
