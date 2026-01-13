using BILET_16_MPA_101.Contexts;
using BILET_16_MPA_101.Models;
using BILET_16_MPA_101.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BILET_16_MPA_101.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrainerController : Controller
    {
        private readonly AppDbContext _context;
      
        public TrainerController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers
                .Include(x => x.Department)
                .Select(x => new TrainerGetVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImagePath = x.ImagePath,
                    DepartmentName = x.Department.Name
                })
                .ToListAsync();

            return View(trainers);
        }


        public async Task<IActionResult> Create()
        {
            await _sendDepartmentsWithViewBag();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrainerCreateVM vm)
        {
            await _sendDepartmentsWithViewBag();

            if (!ModelState.IsValid)
                return View(vm);

            var isExistDepartment = await _context.Departments
                .AnyAsync(x => x.Id == vm.DepartmentId);

            if (!isExistDepartment)
            {
                ModelState.AddModelError("DepartmentId", "This department is not found");
                return View(vm);
            }

            Trainer trainer = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                DepartmentId = vm.DepartmentId,
                ImagePath = vm.ImagePath
            };

            await _context.Trainers.AddAsync(trainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer is null)
                return NotFound();

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);

            if (trainer is null)
                return NotFound();

            TrainerUpdateVM vm = new()
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Description = trainer.Description,
                DepartmentId = trainer.DepartmentId,
                ImagePath = trainer.ImagePath
            };

            await _sendDepartmentsWithViewBag();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TrainerUpdateVM vm)
        {
            await _sendDepartmentsWithViewBag();

            if (!ModelState.IsValid)
                return View(vm);

            var isExistDepartment = await _context.Departments
                .AnyAsync(x => x.Id == vm.DepartmentId);

            if (!isExistDepartment)
            {
                ModelState.AddModelError("DepartmentId", "This department is not found");
                return View(vm);
            }

            var existTrainer = await _context.Trainers.FindAsync(vm.Id);

            if (existTrainer is null)
                return BadRequest();

            existTrainer.Name = vm.Name;
            existTrainer.Description = vm.Description;
            existTrainer.DepartmentId = vm.DepartmentId;
            existTrainer.ImagePath = vm.ImagePath; // string olaraq update

            _context.Trainers.Update(existTrainer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task _sendDepartmentsWithViewBag()
        {
            var departments = await _context.Departments
                .Select(d => new SelectListItem()
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToListAsync();

            ViewBag.Departments = departments;
        }
    }
}
