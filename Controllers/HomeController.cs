using Company.Domain;
using Company.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Company.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public HomeController(UserManager<User> userMgr, SignInManager<User> signInMgr)
    {
        userManager = userMgr;
        signInManager = signInMgr;
    }

    public IActionResult Index()
    {
        return View(userManager.Users);
    }

    public async Task<IActionResult> Update(string? name)
    {
        await UserStatusToggleAsync(name);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string? name)
    {
        await UserRemoveAsync(name);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> UpdateAll(IEnumerable<User> list)
    {
        foreach(var user in list)
        {
            await UserStatusToggleAsync(user.UserName);
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteAll(IEnumerable<User> list)
    {
        foreach(var user in list)
        {
            await UserRemoveAsync(user.UserName);
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task UserStatusToggleAsync(string? name)
    {
        if (name != default)
        {
            User user = await userManager.FindByNameAsync(name);

            if (user != default)
            {
                user.Status = user.Status == UserStatus.BLOCKED ?
                    UserStatus.OFFLINE : UserStatus.BLOCKED;
                await userManager.UpdateAsync(user);
            }
        }
    }

    private async Task UserRemoveAsync(string? name)
    {
        if (name != default)
        {
            User user = await userManager.FindByNameAsync(name);

            if (user != default)
            {
                await userManager.DeleteAsync(user);
            }
        }
    }
}