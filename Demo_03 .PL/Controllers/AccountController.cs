using Demo_03.DAL.Models;
using Demo_03_.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Demo_03_.PL.Controllers
{
    [AllowAnonymous] // => Default
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Actions We Need :- [ Register & Login & Sign Out & Access Denied ] 


        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)  // Server Side Validation
            {
                // Manual Mapping
                //var User = new IdentityUser()
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    Fname = model.FName,
                    Lname = model.LName,
                    IAgree = model.IAgree
                };

                var Result = await _userManager.CreateAsync(User, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }

                else
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        /// Represents in View by :- 
                        /// <div asp-validation-summary="All" class="text-danger"></div>
                    }
                }
            }
            return View(model);
        }

        #endregion


        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(email: model.Email);

                if (User is not null) // Email Check
                {
                    var Flag = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (Flag)
                    {
                        var Result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
                        if (Result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // return View(model);  => مش محتاجها عشان مستخدمها تحت
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password is not Correct");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not Exsits");
                }

            }

            return View(model);
        }

        #endregion


        #region Sign Out

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion


        // Only Admins have the permission to access Users and Roles 

        #region Access Denied
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion  

    }

}
