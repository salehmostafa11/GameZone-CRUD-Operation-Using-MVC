namespace GameZone.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterFormViewModel model)
        {
            if(!ModelState.IsValid || await _userManager.FindByEmailAsync(model.Email)!= null)
            {
                ModelState.AddModelError("", "This email is already in use.");
                return View(model);
            }
            //user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email= model.Email
            };
            var result = await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user,isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginFormViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RemmemberMe,lockoutOnFailure:true);

                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                        return RedirectToAction("Index", "Games");
                    else
                        return RedirectToAction("Index", "Home");
                }

            }

            ModelState.AddModelError("", "Invalid Email or Password!");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
