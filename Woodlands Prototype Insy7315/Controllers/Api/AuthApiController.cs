using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Woodlands_Prototype_Insy7315.Models;
using Woodlands_Prototype_Insy7315.Services;

namespace Woodlands_Prototype_Insy7315.Controllers.Api
{
    // Stateless JWT authentication for API consumers (the Android app,
    // future integrations). This is separate from AccountController, which
    // signs browser users into the cookie-based MVC session. Both talk to
    // the same Identity store, so a password set on one works on the other.
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuditLogService _auditLog;

        public AuthApiController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService,
            IAuditLogService auditLog)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _auditLog = auditLog;
        }

        public class LoginRequest
        {
            [Required, EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            public string Password { get; set; } = "";
        }

        public class RegisterRequest
        {
            [Required, StringLength(100, MinimumLength = 2)]
            public string FullName { get; set; } = "";

            [Required, EmailAddress]
            public string Email { get; set; } = "";

            [Phone]
            public string? Phone { get; set; }

            // Identity's password validators (configured in Program.cs) enforce
            // length/complexity server-side regardless of what the client sends.
            [Required, MinLength(8)]
            public string Password { get; set; } = "";
        }

        public class TokenResponse
        {
            public string Token { get; set; } = "";
            public DateTime ExpiresAtUtc { get; set; }
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public IList<string> Roles { get; set; } = new List<string>();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                await _auditLog.LogAsync("ApiLogin", success: false, subject: request.Email, details: "No account with this email.");
                return Unauthorized(new { error = "Invalid email or password." });
            }

            // CheckPasswordSignInAsync verifies the hashed password and also
            // enforces lockout after repeated failures.
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

            if (result.IsLockedOut)
            {
                await _auditLog.LogAsync("ApiLogin", success: false, subject: request.Email, userId: user.Id, details: "Account locked out.");
                return Unauthorized(new { error = "This account is temporarily locked due to repeated failed attempts." });
            }

            if (!result.Succeeded)
            {
                await _auditLog.LogAsync("ApiLogin", success: false, subject: request.Email, userId: user.Id, details: "Invalid password.");
                return Unauthorized(new { error = "Invalid email or password." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenService.CreateToken(user, roles);

            await _auditLog.LogAsync("ApiLogin", success: true, subject: request.Email, userId: user.Id);

            return Ok(new TokenResponse
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(60),
                FullName = user.FullName,
                Email = user.Email ?? "",
                Roles = roles
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
            {
                await _auditLog.LogAsync("ApiRegister", success: false, subject: request.Email, details: "Email already registered.");
                return Conflict(new { error = "An account with this email address already exists." });
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = InputSanitizer.StripHtml(request.FullName) ?? "",
                PhoneNumber = request.Phone
            };

            // PasswordHasher (PBKDF2) runs inside CreateAsync - the plaintext
            // password is never persisted or logged.
            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description);
                await _auditLog.LogAsync("ApiRegister", success: false, subject: request.Email, details: string.Join("; ", errors));
                return BadRequest(new { errors });
            }

            await _userManager.AddToRoleAsync(user, IdentitySeederRoles.Customer);
            await _auditLog.LogAsync("ApiRegister", success: true, subject: request.Email, userId: user.Id);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenService.CreateToken(user, roles);

            return Ok(new TokenResponse
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(60),
                FullName = user.FullName,
                Email = user.Email ?? "",
                Roles = roles
            });
        }

        // Simple authenticated endpoint used to demonstrate that a bearer
        // token is required and correctly resolves to the calling user.
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Me()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { user.Id, user.Email, user.FullName, user.Branch, Roles = roles });
        }
    }
}
