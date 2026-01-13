using BILET_16_MPA_101.Contexts;
using BILET_16_MPA_101.Models;
using BILET_16_MPA_101.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BILET_16_MPA_101.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        // Index
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Select(d => new DepartmentGetVM
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();

            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var isExist = await _context.Departments.AnyAsync(d => d.Name == vm.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This department already exists");
                return View(vm);
            }

            Department department = new()
            {
                Name = vm.Name
            };

            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department is null)
                return NotFound();

            DepartmentUpdateVM vm = new()
            {
                Id = department.Id,
                Name = department.Name
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(DepartmentUpdateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var existDepartment = await _context.Departments.FindAsync(vm.Id);
            if (existDepartment is null)
                return BadRequest();

            existDepartment.Name = vm.Name;
            _context.Departments.Update(existDepartment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department is null)
                return NotFound();

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
