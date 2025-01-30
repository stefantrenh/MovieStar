using Microsoft.AspNetCore.Identity;

namespace MovieStar.Data.Identity
{
    public class AdminUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
