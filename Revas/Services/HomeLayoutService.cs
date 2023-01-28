using Microsoft.AspNetCore.Identity;
using Revas.Models;

namespace Revas.Services
{
    public class HomeLayoutService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HomeLayoutService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
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