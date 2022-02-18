using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Company.Models;
using Company.Domain;
using Company.Domain.Entities;
using System.Linq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Company.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public AccountController(UserManager<User> userMgr, SignInManager<User> signInMgr)
    {
        userManager = userMgr;
        signInManager = signInMgr;
    }

    [AllowAnonymous]
    public IActionResult Signin()
    {
        return View();
    }

    [AllowAnonymous, HttpPost]
    public async Task<IActionResult> Signin(SigninViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await userManager.FindByNameAsync(model.UserName);
            if(user == default)
            {
                user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    NormalizedUserName = model.UserName.ToUpper(),
                    Email = model.UserEmail,
                    Published = DateTime.UtcNow,
                    Status = UserStatus.OFFLINE,
                    PasswordHash = new PasswordHasher<User>().HashPassword(null, model.UserPassword),
                };
                await userManager.CreateAsync(user);
                return RedirectToAction(nameof(Login), new LoginViewModel 
                { 
                    UserName = user.UserName, 
                    ReturnUrl = "/"
                });
            }
            ModelState.AddModelError("", "User is exist");
        }
        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginViewModel() { ReturnUrl = returnUrl });
    }

    [AllowAnonymous, HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await userManager.FindByNameAsync(model.UserName);

            if (user != default)
            {
                await signInManager.SignOutAsync();
                SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password ?? "", false, false);
                if (result.Succeeded)
                {
                    user.Status = UserStatus.ONLINE;
                    user.LastLogin = DateTime.UtcNow;
                    await userManager.UpdateAsync(user);
                    return Redirect(model.ReturnUrl ?? "/");
                }
            }
        }
        ModelState.AddModelError(nameof(LoginViewModel.UserName), "Login or password not correct");
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        User user = await userManager.FindByNameAsync(User.Identity?.Name);
        user.Status = UserStatus.OFFLINE;
        await userManager.UpdateAsync(user);
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
