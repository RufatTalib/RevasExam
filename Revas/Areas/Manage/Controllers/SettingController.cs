using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revas.DAL;
using Revas.VMs;

namespace Revas.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SettingController : Controller
    {
        private readonly RevasContext _context;

        public SettingController(RevasContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            SettingVM vm = new SettingVM
            {
                ProjectName = _context.Settings.FirstOrDefault(x => x.Key == "ProjectName").Value,
                Location = _context.Settings.FirstOrDefault(x => x.Key == "Location").Value
            };

            return View(vm);
        }

        /*public IActionResult Init()
        {
            _context.Settings.Add(new Setting { Key = "ProjectName", Value = "Revas" });
            _context.Settings.Add(new Setting { Key = "Location", Value = "230/45 , Newyork City, USA-305670" });
            _context.SaveChanges();

            return Ok();
        }*/

        [HttpPost]
        public IActionResult Save(SettingVM model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.ProjectName != null)
            {
                _context.Settings.FirstOrDefault(x => x.Key == "ProjectName").Value = model.ProjectName;
                _context.SaveChanges();
            }
            if (model.Location != null)
            {
                _context.Settings.FirstOrDefault(x => x.Key == "Location").Value = model.Location;
                _context.SaveChanges();
            }

            return View(model);
        }
    }
}
