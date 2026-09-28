using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Woodlands_Prototype_Insy7315.Models;
using Woodlands_Prototype_Insy7315.Services;

namespace Woodlands_Prototype_Insy7315.Controllers
{
    // Browser-facing login/register for the MVC site. Authenticates against
    // the local ASP.NET Core Identity store (UserManager/SignInManager), so
    // passwords are hashed with Identity's PBKDF2 hasher and role checks use
    // the same AspNetRoles table the rest of the app relies on. This used to
    // proxy through an external Node/Supabase service; that dependency has
    // been removed so login/registration work without it.
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuditLogService _auditLog;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuditLogService auditLog,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _auditLog = auditLog;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                await _auditLog.LogAsync("Login", success: false, subject: model.Email, details: "No account with this email.");
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // SignInManager handles the hashed-password comparison and
            // enforces lockout after repeated failed attempts (configured in
            // Program.cs), which protects against brute-force login attempts.
            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                await _auditLog.LogAsync("Login", success: false, subject: model.Email, userId: user.Id, details: "Account locked out.");
                ModelState.AddModelError("", "This account is temporarily locked due to repeated failed attempts. Please try again later.");
                return View(model);
            }

            if (!result.Succeeded)
            {
                await _auditLog.LogAsync("Login", success: false, subject: model.Email, userId: user.Id, details: "Invalid password.");
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // Add the app-specific claims (FullName, Branch) on top of the
            // ones SignInManager already added (NameIdentifier, Name, Role).
            await _signInManager.SignInWithClaimsAsync(user, model.RememberMe, await BuildExtraClaimsAsync(user));

            await _auditLog.LogAsync("Login", success: true, subject: model.Email, userId: user.Id);

            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(
                "Index",
                "Dashboard");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
            {
                await _auditLog.LogAsync("Register", success: false, subject: model.Email, details: "Email already registered.");
                ModelState.AddModelError("", "An account with this email address already exists.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = InputSanitizer.StripHtml(model.FullName) ?? "",
                PhoneNumber = model.PhoneNumber
            };

            // Identity validates password strength server-side (Program.cs
            // Identity.Password options) and stores only the PBKDF2 hash.
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                await _auditLog.LogAsync("Register", success: false, subject: model.Email,
                    details: string.Join("; ", result.Errors.Select(e => e.Description)));

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, IdentitySeederRoles.Customer);
            await _auditLog.LogAsync("Register", success: true, subject: model.Email, userId: user.Id);

            TempData["RegistrationMessage"] =
                "Your account has been created. You can now sign in.";

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _signInManager.SignOutAsync();
            await _auditLog.LogAsync("Logout", success: true, userId: userId);

            return RedirectToAction(
                "Index",
                "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task<IEnumerable<Claim>> BuildExtraClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new("FullName", user.FullName)
            };

            if (!string.IsNullOrWhiteSpace(user.Branch))
            {
                claims.Add(new Claim("Branch", user.Branch));
            }

            await Task.CompletedTask;
            return claims;
        }
    }
}
