using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserViewModel
    {
        [Required(ErrorMessage = "Please fill up username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please fill up password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
