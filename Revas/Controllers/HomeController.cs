using Microsoft.AspNetCore.Mvc;
using Revas.DAL;
using Revas.VMs;

namespace Revas.Controllers
{
    public class HomeController : Controller
    {
        private readonly RevasContext _context;

        public HomeController(RevasContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM model = new HomeVM
            {
                Portfolios = _context.Portfolios.Where(x => x.IsDeleted == false).ToList(),
                Employees = _context.Employees.Where(x => x.IsDeleted == false).ToList()
            };

            return View(model);
        }


    }
}
