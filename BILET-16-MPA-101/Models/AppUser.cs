using Microsoft.AspNetCore.Identity;

namespace BILET_16_MPA_101.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; } = string.Empty;
    }
}
