using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HRMSDbContext _context;

        public EmployeesController(HRMSDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.Include(e => e.Department).Include(e => e.Position).ToListAsync();
            return View(employees);
        }

        
        public IActionResult Create()
        {
            var departments = _context.Departments.ToList();
            var positions = _context.Positions.ToList();

            if (departments == null || !departments.Any())
            {
               
            }

            if (positions == null || !positions.Any())
            {
                
            }

            ViewBag.Departments = departments;
            ViewBag.Positions = positions;

            return View();
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateEmployeeDTO employeeDto)
        {
            if (ModelState.IsValid)
            {
               
                var employee = new Employee
                {
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    Email = employeeDto.Email,
                    DateOfBirth = employeeDto.DateOfBirth,
                    HireDate = employeeDto.HireDate,
                    PhoneNumber = employeeDto.PhoneNumber,
                    Address = employeeDto.Address,
                    Salary = employeeDto.Salary,
                    DepartmentId = employeeDto.DepartmentId,
                    PositionId = employeeDto.PositionId
                };

                _context.Employees.Add(employee);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

           
            return View(employeeDto);
        }


        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

          
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();

            // Map the employee object to an EditEmployeeDTO
            var response = new EditEmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                DateOfBirth = employee.DateOfBirth,
                HireDate = employee.HireDate,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                PositionId = employee.PositionId
            };

            return View(response); 
        }


      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEmployeeDTO model)
        {
            if (!ModelState.IsValid)
            {
                
                ViewBag.Departments = _context.Departments.ToList();
                ViewBag.Positions = _context.Positions.ToList();
                return View(model); 
            }

           
            var employee = await _context.Employees.FindAsync(model.Id);
            if (employee == null)
            {
                return NotFound();
            }

           
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Email = model.Email;
            employee.DateOfBirth = model.DateOfBirth;
            employee.HireDate = model.HireDate;
            employee.PhoneNumber = model.PhoneNumber;
            employee.Address = model.Address;
            employee.Salary = model.Salary;
            employee.DepartmentId = model.DepartmentId;
            employee.PositionId = model.PositionId;

            _context.Update(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); 
        }

        
        public IActionResult Details(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Department) 
                .Include(e => e.Position)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
    

    // GET: Employees/Delete/5
    public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
