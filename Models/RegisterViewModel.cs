using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class RegisterViewModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    
}