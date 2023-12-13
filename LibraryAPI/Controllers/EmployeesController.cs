using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryAPI.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<Employee> _passwordHasher;

        public EmployeesController(AppDbContext dbContext, IConfiguration configuration, IPasswordHasher<Employee> passwordHasher)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(_dbContext.Employees.ToList());
        }

        // get id eployee
        [HttpGet("{id}")]
        public IActionResult GetEmployee([FromRoute] Guid id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }


        [HttpPost]
        public IActionResult CreateEmployee([FromBody] EmployeeDto employee)
        {
            var newEmployee = new Employee()
            {
                Name = employee.Name,
                Surname = employee.Surname,
                JobPosition = employee.JobPosition,

            };
            newEmployee.PasswordHash = _passwordHasher.HashPassword(newEmployee, employee.Password);

            _dbContext.Employees.Add(newEmployee);
            _dbContext.SaveChanges();
            return Ok(_dbContext.Employees.FirstOrDefault(e => e.Name == newEmployee.Name && e.Surname == newEmployee.Surname));
        }



        [HttpPut("{id}")] //put = edit
        public IActionResult EditEmployee([FromRoute] Guid id, EmployeeDto employee)
        {
            var originalEmployee = _dbContext.Employees.FirstOrDefault(r => r.Id == id);
            if (originalEmployee == null)
            {
                return NotFound();
            }

            
            originalEmployee.Name = employee.Name;
            originalEmployee.Surname = employee.Surname;
            originalEmployee.JobPosition = employee.JobPosition;
            originalEmployee.PasswordHash = _passwordHasher.HashPassword(originalEmployee, employee.Password);


            _dbContext.SaveChanges();
            return Ok(_dbContext.Employees.FirstOrDefault(r => r.Id == id));
        }

        // delete id employee
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee([FromRoute] Guid id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();
            return Ok();
        }


    }
}
