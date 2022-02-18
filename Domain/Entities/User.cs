using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Company.Domain.Entities;

public class User : IdentityUser
{
    [Display(Name = "Last login")]
    public DateTime LastLogin { get; set; }

    [Display(Name = "Published")]
    public DateTime Published { get; set; }

    [Display(Name = "Account status")]
    public UserStatus Status { get; set; }
}
