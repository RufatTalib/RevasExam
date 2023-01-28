using Microsoft.AspNetCore.Identity;

namespace Revas.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }

    }
}
