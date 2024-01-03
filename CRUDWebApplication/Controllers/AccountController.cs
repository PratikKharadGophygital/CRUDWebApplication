using Entities.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDWebApplication.Controllers
{
    [Route("[Controller]/[action]")]
    //[AllowAnonymous] If we are working with custom policy remove the allowanonymous on controller level.
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize("NotAuthorized")] // Custom policy name 
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")] // Custom policy name 
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };

            IdentityResult result = await _userManager.CreateAsync(applicationUser, registerDTO.Password);

            if (result.Succeeded)
            {
                if (registerDTO.UserType == UserTypeOptions.Admin)
                {
                    // Create 'Admin' role if role manager not found the admin role 
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = UserTypeOptions.Admin.ToString()

                        };
                        await _roleManager.CreateAsync(applicationRole);                      
                       
                    }
                    // Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(applicationUser, UserTypeOptions.Admin.ToString());
                }               
                else
                {
                    // Add the new user into 'User' role
                    await _userManager.AddToRoleAsync(applicationUser, UserTypeOptions.User.ToString());
                }              
               
                await _signInManager.SignInAsync(applicationUser,isPersistent:true);
                return RedirectToAction(nameof(PersonsController.Index), "Persons");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
            }

            return View(registerDTO);
        }

        [HttpGet]
        [Authorize("NotAuthorized")] // Custom policy name 
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")] // Custom policy name 
        public async Task<IActionResult> Login(LoginDTO loginDTO,string? ReturnUrl)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);

                return View(loginDTO);
            }
             
           var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password,isPersistent:true,lockoutOnFailure:true);

            if (result.Succeeded)
            {
                ApplicationUser applicationUser = await _userManager.FindByNameAsync(loginDTO.Email);
                if(applicationUser != null)
                {
                    if (await _userManager.IsInRoleAsync(applicationUser,UserTypeOptions.Admin.ToString()))
                    {
                        // Mention this area for the find the admin controller 
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }

                // Help to cross posting attack if only redirect then same domain found 
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    // Local meas the same domain name direct
                    return LocalRedirect(ReturnUrl);
                } 
            }

            ModelState.AddModelError("Login", "Invalid email or password");


            return View(loginDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonsController.Index), "Persons");
        }

        public async Task<IActionResult> IsEmailAlreadtRegistered(string email)
        {
            ApplicationUser user =  await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Browser only recive the json result 
                // email address Valid 
                return Json(true);
            }
            else
            {
                // Invalid email address 
                return Json(false);
            }
        }

    }
}
