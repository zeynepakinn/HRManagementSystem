using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly HRMSDbContext _context;

        public DepartmentsController(HRMSDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var departments = _context.Departments.Take(20).ToList();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDTO department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(new Department() { Name=department.Name,Description=department.Description});
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }



            
            return View(department);
        }


       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var response = new EditDepartmentDTO
            {
                Id = department.Id,
                Name = department.Name, 
                Description = department.Description                       
            };

            
            return View(response);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDepartmentDTO departmentDto)
        {
            if (id != departmentDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var department = await _context.Departments.FindAsync(id);

                    if (department == null)
                    {
                        return NotFound();
                    }

                    
                    department.Name = departmentDto.Name;
                    department.Description = departmentDto.Description;

                    
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(departmentDto.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departmentDto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Employees) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }



        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
