using Microsoft.AspNetCore.Identity;
using Revas.DAL;
using Revas.Models;
using Revas.VMs;

namespace Revas.Services
{
    public class HomeLayoutService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RevasContext _context;

        public HomeLayoutService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, RevasContext context)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<AppUser> GetCurrentUser()
        {
            var Identity = httpContextAccessor.HttpContext.User.Identity;

            if (Identity.IsAuthenticated)
                return await userManager.FindByNameAsync(Identity.Name);

            return null;
        }

        public SettingVM GetCurrentSetting()
        {
            SettingVM vm = new SettingVM
            {
                ProjectName = _context.Settings.FirstOrDefault(x => x.Key == "ProjectName").Value,
                Location = _context.Settings.FirstOrDefault(x => x.Key == "Location").Value
            };
            return vm;
        }
    }
}