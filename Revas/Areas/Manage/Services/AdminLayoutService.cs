using Microsoft.AspNetCore.Identity;
using Revas.Models;

namespace Revas.Areas.Manage.Services
{
    public class AdminLayoutService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AdminLayoutService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<AppUser> GetCurrentUser()
        {
            var Identity = httpContextAccessor.HttpContext.User.Identity;

            if (Identity.IsAuthenticated)
                return await userManager.FindByNameAsync(Identity.Name);

            return null;
        }
    }
}
