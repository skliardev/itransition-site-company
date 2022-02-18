using System.ComponentModel.DataAnnotations;

namespace Company.Models;

public class LoginViewModel
{
    [Display(Name = "Login"), Required]
    public string UserName { get; set; }

    [Display(Name = "Password"), UIHint("password"), Required]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }
}
