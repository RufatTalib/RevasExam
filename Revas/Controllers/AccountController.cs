using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Revas.Models;
using Revas.VMs;

namespace Revas.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        // Username Admin123
        // Password Admin123
        // Role SuperAdmin
        /*public async Task<IActionResult> Initialize()
        {
            //Firstly create roles
            await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });


            AppUser user = new AppUser
            {
                UserName = "Admin123",
                Fullname = "Super Admin"
            };

            await _userManager.CreateAsync(user, "Admin123");
            await _userManager.AddToRoleAsync(user, "SuperAdmin");

            return Content("Initialized");
        }*/

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            AppUser user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is invalid!");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is invalid!");
                return View(model);
            }

            return RedirectToAction("index", "home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            AppUser user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                ModelState.AddModelError("UserName", "Username already exists.");
                return View(model);
            }

            user = new AppUser
            {
                UserName = model.UserName,
                Fullname = model.FullName
            };

            var response = await _userManager.CreateAsync(user, model.Password);

            if (!response.Succeeded)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("login");
        }

    }
}
