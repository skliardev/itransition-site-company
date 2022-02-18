using System.ComponentModel.DataAnnotations;

namespace Company.Models;

public class SigninViewModel
{
    [Display(Name = "User name"), Required]
    public string UserName { get; set; }

    [Display(Name = "Email"), Required]
    public string UserEmail { get; set; }

    [Display(Name = "Password"), UIHint("password"), Required]
    public string UserPassword { get; set; }
}
