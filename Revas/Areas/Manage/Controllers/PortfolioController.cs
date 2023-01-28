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
    public class PortfolioController : Controller
    {
        private readonly RevasContext _context;
        private readonly IWebHostEnvironment _env;

        public PortfolioController(RevasContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public List<Portfolio> Portfolios { get => _context.Portfolios.Where(x => x.IsDeleted == false).ToList(); }

        public IActionResult Index(int page = 1)
        {
            var query = _context.Portfolios.Where(x => x.IsDeleted == false).AsQueryable();

            PaginatedList<Portfolio> paginatedList = PaginatedList<Portfolio>.Create(query, page, 3);

            return View(paginatedList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Portfolio portfolio)
        {
            if (!ModelState.IsValid) return View(portfolio);

            if (portfolio.Image is null)
            {
                ModelState.AddModelError("Image", "You must upload a photo!");
                return View(portfolio);
            }

            string FileFormat = portfolio.Image.ContentType.ToLower();

            if (FileFormat != "image/jpg" && FileFormat != "image/jpeg" && FileFormat != "image/png")
            {
                ModelState.AddModelError("Image", "You must upload a photo!");
                return View(portfolio);
            }


            portfolio.ImageUrl = portfolio.Image.Save(_env.WebRootPath, "upload");

            _context.Portfolios.Add(portfolio);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int Id)
        {
            Portfolio portfolio = Portfolios.FirstOrDefault(x => x.Id == Id);

            if (portfolio == null)
                return NotFound();

            return View(portfolio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Portfolio newPortfolio)
        {
            if (!ModelState.IsValid) return View(newPortfolio);

            Portfolio portfolio = Portfolios.FirstOrDefault(x => x.Id == newPortfolio.Id);

            if (newPortfolio.Image != null)
            {
                string FileFormat = newPortfolio.Image.ContentType.ToLower();

                if (FileFormat != "image/jpg" && FileFormat != "image/jpeg" && FileFormat != "image/png")
                {
                    ModelState.AddModelError("Image", "You must upload a photo!");
                    return View(portfolio);
                }
                // newPortfolio instead of exist portfolio because
                // reflection will collect this data for us

                newPortfolio.ImageUrl = newPortfolio.Image.Save(_env.WebRootPath, "upload");

                //We dont need it anymore ! :)
                newPortfolio.Image = null;
            }

            portfolio.SmartUpdate(newPortfolio);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int Id)
        {
            Portfolio portfolio = Portfolios.FirstOrDefault(x => x.Id == Id);

            if (null == portfolio)
                return NotFound();


            //Soft Delete !
            portfolio.IsDeleted = true;
            _context.SaveChanges();

            return Ok();
        }



    }
}
