using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revas.DAL;
using Revas.Helpers;
using Revas.Models;
using System.Data;

namespace Revas.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EmployeeController : Controller
    {
        private readonly RevasContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(RevasContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public List<Employee> Employees { get => _context.Employees.Where(x => x.IsDeleted == false).ToList(); }

        public IActionResult Index(int page = 1)
        {
            var query = _context.Employees.Where(x => x.IsDeleted == false).AsQueryable();

            PaginatedList<Employee> paginatedList = PaginatedList<Employee>.Create(query, page, 3);

            return View(paginatedList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (!ModelState.IsValid) return View(employee);

            if (employee.Image is null)
            {
                ModelState.AddModelError("Image", "You must upload a photo!");
                return View(employee);
            }

            string FileFormat = employee.Image.ContentType.ToLower();

            if (FileFormat != "image/jpg" && FileFormat != "image/jpeg" && FileFormat != "image/png")
            {
                ModelState.AddModelError("Image", "You must upload a photo!");
                return View(employee);
            }

            employee.ImageUrl = employee.Image.Save(_env.WebRootPath, "upload");

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int Id)
        {
            Employee employee = _context.Employees.FirstOrDefault(x => x.Id == Id);

            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee newEmployee)
        {
            if (!ModelState.IsValid) return View(newEmployee);

            Employee employee = Employees.FirstOrDefault(x => x.Id == newEmployee.Id);

            if (newEmployee.Image != null)
            {
                string FileFormat = newEmployee.Image.ContentType.ToLower();

                if (FileFormat != "image/jpg" && FileFormat != "image/jpeg" && FileFormat != "image/png")
                {
                    ModelState.AddModelError("Image", "You must upload a photo!");
                    return View(employee);
                }

                // Reflection will collect all data from newEmployee and set to exist employee
                // so we should prepare newEmployee for reflection !!!
                newEmployee.ImageUrl = newEmployee.Image.Save(_env.WebRootPath, "upload");

                // We don't need anymore, so we should hide this for reflection update (SmartUpdate)
                newEmployee.Image = null;
            }

            employee.SmartUpdate(newEmployee);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int Id)
        {
            Employee employee = Employees.FirstOrDefault(x => x.Id == Id);

            if (employee == null)
                return NotFound();

            employee.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }

    }
}
