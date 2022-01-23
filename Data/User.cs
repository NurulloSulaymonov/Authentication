using Microsoft.AspNetCore.Identity;

namespace Auth.Data
{
    public class User : IdentityUser
    {
        public string Surname { get; set; }
        public string ProfileImage { get; set; }
    }
}
