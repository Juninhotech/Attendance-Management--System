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
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext _employeeDbContext;
        private object[] StaffID;

        public EmployeeController(EmployeeDbContext employeeDbContext)
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
        public async Task<IActionResult> GetAllLogin()
        {
            var employee = await _employeeDbContext.EmployeeDb.Select(x => new EmployeeViewModel
            {
                Id = x.Id,
                StaffID = x.StaffID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Department = x.Department,
                PunchIn = x.PunchIn.ToShortDateString() + " " + x.PunchIn.ToShortTimeString(),
                PunchOut = x.PunchOut.Year == 0001 ? "On Duty" : x.PunchOut.ToShortDateString() + " " + x.PunchOut.ToShortTimeString()

            }).ToListAsync();

            return Ok(employee);


        }


       [HttpPost]
       [Route("{Login}")]

        public async Task<IActionResult> SignInStaff([FromBody] PunchViewModel LogStaffIn)
        {

            ErrorViewModel err = new ErrorViewModel();

            try
            {
                var isStaff = await _employeeDbContext.WorkersDb.Where(x => x.StaffID == LogStaffIn.StaffID).AnyAsync();

                if (isStaff)
                {
                    var staff = await _employeeDbContext.WorkersDb.Where(x => x.StaffID == LogStaffIn.StaffID && x.Password == HashedPassword(LogStaffIn.Password)).FirstOrDefaultAsync();
                    if (HashedPassword(LogStaffIn.Password) == staff.Password)
                    {

                        var signedInToday = await _employeeDbContext.EmployeeDb.Where(x => x.StaffID == LogStaffIn.StaffID && x.PunchIn.Date == DateTime.Now.Date).AnyAsync();

                        if (!signedInToday)
                        {
                            err.id = staff.id;
                            err.fname = $"{staff.FirstName}";
                            err.Message = $"You signed in at {DateTime.Now.ToShortTimeString()}";
                            err.code = "01"; 

                            var staffLog = new Employee
                            {
                                StaffID = staff.StaffID,
                                FirstName = staff.FirstName,
                                LastName = staff.LastName,
                                Department = staff.Department,
                                PunchIn = DateTime.Now,
                                PunchOut = new DateTime(), 
                       
    
                            };

                            await _employeeDbContext.EmployeeDb.AddAsync(staffLog);
                            await _employeeDbContext.SaveChangesAsync();

                            return Json(err);
                        }
                        else
                        {
                        
                            err.Message = "You've already signed in";
                            err.code = "02";
                            return Json(err);
                        }

                    }
                    else
                    {
                        err.Message = "Invalid credentials";
                        err.code = "03";
                        return Json(err);
                    }
                   
                }
                else
                {
                    err.Message = "This staff doesn't exist";
                    err.code = "04";
                    return Json(err);
                }
            }
            catch (Exception)
            {
                 
                 err.Message = "An error occurs";
                 err.code = "05";
                 return Json(err);
               
            }

           
           


        }

        [HttpGet]
        [Route("{staffID}")]

        public async Task<IActionResult> GetAllWorkers([FromRoute] string staffID)
        {
            var logDetails = await _employeeDbContext.EmployeeDb.FirstOrDefaultAsync(x => x.StaffID == staffID);

            if (logDetails == null)
            {
                return NotFound("Staff doesn't exist");
            }
            return Ok(logDetails);
        }



        [HttpPut]
        [Route("{staffID}")]
        public async Task<IActionResult> LogMeOut([FromRoute] string staffID, PunchOutVIewModel logMeOut)
        {
            ErrorViewModel prr = new ErrorViewModel();
            try
            {

                var punchmeOut = await _employeeDbContext.EmployeeDb.FirstOrDefaultAsync(x => x.StaffID == staffID);

                if (punchmeOut != null)
                {
                    var LogedOutToday = await _employeeDbContext.EmployeeDb.Where(x => x.StaffID == logMeOut.StaffID && x.PunchOut.Date == DateTime.Now.Date).AnyAsync();

                    if (!LogedOutToday)
                    {
                        prr.code = "00";
                        prr.fname = punchmeOut.FirstName;
                        prr.Message = "You've signed out for day, have a nice day";
                        punchmeOut.PunchOut = DateTime.Now;

                        _employeeDbContext.Entry(punchmeOut).State = EntityState.Modified;
                        await _employeeDbContext.SaveChangesAsync();

                        return Json(prr);

                    }

                    else
                    {
                        prr.code = "01";
                        prr.fname = punchmeOut.FirstName;
                        prr.Message = $"Hello You've already signed out";
                        return Ok(prr);
                    }
                }

                else
                {
                    prr.code = "02";
                    prr.Message = "Invalid details ";
                    return Ok(prr);
                }

            }
            catch (Exception)
            {
                prr.code = "03";
                prr.Message = "Error occur"; 

            }


            return Ok();

        }

        


    }
}
