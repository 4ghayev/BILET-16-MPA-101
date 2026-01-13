using BILET_16_MPA_101.Models;
using BILET_16_MPA_101.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace MPA101_Simulation.Controllers;
public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager) : Controller
{
    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var existingUser = await _userManager.FindByEmailAsync(vm.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("Email", "This email is already registered.");
            return View(vm);
        }

        var user = new AppUser
        {
            UserName = vm.Username,
            Email = vm.Email,
            Fullname = vm.Fullname
        };

        var result = await _userManager.CreateAsync(user, vm.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(vm);
        }

        await _userManager.AddToRoleAsync(user, "Member");

        return RedirectToAction("Login");
    }



    public IActionResult Login()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);



        var user = await _userManager.FindByEmailAsync(vm.Email);

        if (user is null)
        {
            ModelState.AddModelError("", "Username or password is wrong");
            return View(vm);
        }


        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, true);


        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Username or password is wrong");
            return View(vm);
        }




        return RedirectToAction("Index", "Home");

    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();


        return RedirectToAction("Login");

    }


    public async Task<IActionResult> CreateRoles()
    {
        await _roleManager.CreateAsync(new IdentityRole()
        {
            Name = "Admin"
        });
        await _roleManager.CreateAsync(new IdentityRole()
        {
            Name = "Member"
        });


        return Ok("Role was created");
    }


}