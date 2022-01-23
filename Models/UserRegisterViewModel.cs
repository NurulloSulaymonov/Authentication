using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Please fill up username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please fill up password")]
        public string Password { get; set; }
        
        [Compare("Password",ErrorMessage ="your password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
