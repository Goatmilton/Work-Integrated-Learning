using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Woodlands_Prototype_Insy7315.Models;
using Woodlands_Prototype_Insy7315.Services;

namespace Woodlands_Prototype_Insy7315.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly ILogger<AdminController> _logger;
        private readonly IAuditLogService _auditLog;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        public AdminController(
            IHttpClientFactory http,
            ILogger<AdminController> logger,
            IAuditLogService auditLog,
            UserManager<ApplicationUser> userManager)
        {
            _http = http;
            _logger = logger;
            _auditLog = auditLog;
            _userManager = userManager;
        }

        // ==================== USERS ====================
        // User accounts, passwords and role membership live in ASP.NET Core
        // Identity (AspNetUsers/AspNetRoles), managed here via UserManager.
        // This used to proxy to an external Node/Supabase API for user CRUD;
        // that dependency has been removed so account management works
        // end-to-end without it.

        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.OrderBy(u => u.Email).ToList();
            var rows = new List<AdminUserRowViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                rows.Add(new AdminUserRowViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email ?? "",
                    PhoneNumber = u.PhoneNumber,
                    Role = roles.FirstOrDefault() ?? IdentitySeederRoles.Customer,
                    Branch = u.Branch,
                    Active = !u.LockoutEnabled || u.LockoutEnd == null || u.LockoutEnd < DateTimeOffset.UtcNow
                });
            }

            return View(rows);
        }

        [HttpGet]
        public IActionResult CreateUser() => View("UserForm", new UserFormViewModel { Role = IdentitySeederRoles.Customer, Active = true });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError(nameof(model.Password), "A password is required when creating a user.");

            if (!IdentitySeederRoles.All.Contains(model.Role))
                ModelState.AddModelError(nameof(model.Role), "Invalid role.");

            if (!ModelState.IsValid) return View("UserForm", model);

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email address already exists.");
                return View("UserForm", model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = InputSanitizer.StripHtml(model.FullName) ?? "",
                PhoneNumber = model.PhoneNumber,
                Branch = InputSanitizer.StripHtml(model.Branch),
                EmailConfirmed = true,
                LockoutEnabled = true,
                // A disabled account is represented as "locked out forever" -
                // Identity has no separate Active flag, and this reuses the
                // same lockout check PasswordSignInAsync already enforces.
                LockoutEnd = model.Active ? null : DateTimeOffset.MaxValue
            };

            var createResult = await _userManager.CreateAsync(user, model.Password!);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                    ModelState.AddModelError("", error.Description);

                await _auditLog.LogAsync("AdminCreateUser", success: false, subject: model.Email,
                    details: string.Join("; ", createResult.Errors.Select(e => e.Description)));

                return View("UserForm", model);
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            var actingAdmin = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            await _auditLog.LogAsync("AdminCreateUser", success: true, subject: model.Email, userId: actingAdmin,
                details: $"Role: {model.Role}");

            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var isActive = !user.LockoutEnabled || user.LockoutEnd == null || user.LockoutEnd < DateTimeOffset.UtcNow;

            return View("UserForm", new UserFormViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault() ?? IdentitySeederRoles.Customer,
                Branch = user.Branch,
                Active = isActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, UserFormViewModel model)
        {
            if (!IdentitySeederRoles.All.Contains(model.Role))
                ModelState.AddModelError(nameof(model.Role), "Invalid role.");

            if (!ModelState.IsValid) { model.Id = id; return View("UserForm", model); }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.FullName = InputSanitizer.StripHtml(model.FullName) ?? "";
            user.PhoneNumber = model.PhoneNumber;
            user.Branch = InputSanitizer.StripHtml(model.Branch);
            user.Email = model.Email;
            user.UserName = model.Email;
            user.LockoutEnd = model.Active ? null : DateTimeOffset.MaxValue;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);

                await _auditLog.LogAsync("AdminEditUser", success: false, subject: model.Email,
                    details: string.Join("; ", updateResult.Errors.Select(e => e.Description)));

                model.Id = id;
                return View("UserForm", model);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(model.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            var actingAdmin = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            await _auditLog.LogAsync("AdminEditUser", success: true, subject: model.Email, userId: actingAdmin,
                details: $"Target: {id}, Role: {model.Role}, Active: {model.Active}");

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return RedirectToAction(nameof(Users));

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (id == currentUserId)
            {
                TempData["AdminError"] = "You cannot delete the account you are currently using.";
                return RedirectToAction(nameof(Users));
            }

            var actingAdmin = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    TempData["AdminError"] = "That user account could not be found.";
                    return RedirectToAction(nameof(Users));
                }

                var deleteResult = await _userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    TempData["AdminError"] = "Unable to delete the user account.";
                    await _auditLog.LogAsync("AdminDeleteUser", success: false, subject: user.Email, userId: actingAdmin,
                        details: string.Join("; ", deleteResult.Errors.Select(e => e.Description)));
                    return RedirectToAction(nameof(Users));
                }

                await _auditLog.LogAsync("AdminDeleteUser", success: true, subject: user.Email, userId: actingAdmin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {Id}", id);
                await _auditLog.LogAsync("AdminDeleteUser", success: false, subject: id, userId: actingAdmin, details: ex.Message);
                TempData["AdminError"] = "Unable to delete the user account.";
            }

            return RedirectToAction(nameof(Users));
        }

        // ==================== TESTIMONIALS ====================

        public async Task<IActionResult> Testimonials()
        {
            var testimonials = new List<Testimonial>();
            try
            {
                var client = _http.CreateClient("NodeApi");
                var res = await client.GetAsync("api/testimonials");
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    testimonials = JsonSerializer.Deserialize<List<Testimonial>>(json, _json) ?? new();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading testimonials");
                TempData["AdminError"] = "Unable to load testimonials.";
            }
            return View(testimonials);
        }

        [HttpGet]
        public IActionResult CreateTestimonial() => View("TestimonialForm", new TestimonialFormViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTestimonial(TestimonialFormViewModel model)
        {
            if (!ModelState.IsValid) return View("TestimonialForm", model);

            try
            {
                var payload = new
                {
                    name = InputSanitizer.StripHtml(model.Name),
                    role = InputSanitizer.StripHtml(model.Role),
                    location = InputSanitizer.StripHtml(model.Location),
                    rating = model.Rating,
                    review = InputSanitizer.StripHtml(model.Review),
                    project = InputSanitizer.StripHtml(model.Project)
                };

                var client = _http.CreateClient("NodeApi");
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                await client.PostAsync("api/testimonials", content);

                return RedirectToAction(nameof(Testimonials));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating testimonial");
                ModelState.AddModelError("", "Unable to create the testimonial.");
                return View("TestimonialForm", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditTestimonial(int id)
        {
            try
            {
                var client = _http.CreateClient("NodeApi");
                var res = await client.GetAsync("api/testimonials");
                if (!res.IsSuccessStatusCode) return NotFound();

                var json = await res.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<Testimonial>>(json, _json) ?? new();
                var t = list.FirstOrDefault(x => x.Id == id);
                if (t == null) return NotFound();

                return View("TestimonialForm", new TestimonialFormViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Role = t.Role,
                    Location = t.Location,
                    Rating = t.Rating,
                    Review = t.Review,
                    Project = t.Project
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading testimonial {Id}", id);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTestimonial(int id, TestimonialFormViewModel model)
        {
            if (!ModelState.IsValid) { model.Id = id; return View("TestimonialForm", model); }

            try
            {
                var payload = new
                {
                    name = InputSanitizer.StripHtml(model.Name),
                    role = InputSanitizer.StripHtml(model.Role),
                    location = InputSanitizer.StripHtml(model.Location),
                    rating = model.Rating,
                    review = InputSanitizer.StripHtml(model.Review),
                    project = InputSanitizer.StripHtml(model.Project)
                };

                var client = _http.CreateClient("NodeApi");
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                await client.PutAsync($"api/testimonials/{id}", content);

                return RedirectToAction(nameof(Testimonials));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating testimonial {Id}", id);
                ModelState.AddModelError("", "Unable to update the testimonial.");
                model.Id = id;
                return View("TestimonialForm", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            try
            {
                var client = _http.CreateClient("NodeApi");
                await client.DeleteAsync($"api/testimonials/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting testimonial {Id}", id);
            }
            return RedirectToAction(nameof(Testimonials));
        }

        // ==================== FAQS ====================

        public async Task<IActionResult> Faqs()
        {
            var faqs = new List<FaqItem>();
            try
            {
                var client = _http.CreateClient("NodeApi");
                var res = await client.GetAsync("api/faqs");
                if (res.IsSuccessStatusCode)
                {
                    var json = await res.Content.ReadAsStringAsync();
                    faqs = JsonSerializer.Deserialize<List<FaqItem>>(json, _json) ?? new();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading FAQs");
                TempData["AdminError"] = "Unable to load FAQs.";
            }
            return View(faqs);
        }

        [HttpGet]
        public IActionResult CreateFaq() => View("FaqForm", new FaqFormViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaq(FaqFormViewModel model)
        {
            if (!ModelState.IsValid) return View("FaqForm", model);

            try
            {
                var payload = new { category = InputSanitizer.StripHtml(model.Category), question = InputSanitizer.StripHtml(model.Question), answer = InputSanitizer.StripHtml(model.Answer) };

                var client = _http.CreateClient("NodeApi");
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                await client.PostAsync("api/faqs", content);

                return RedirectToAction(nameof(Faqs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating FAQ");
                ModelState.AddModelError("", "Unable to create the FAQ.");
                return View("FaqForm", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditFaq(int id)
        {
            try
            {
                var client = _http.CreateClient("NodeApi");
                var res = await client.GetAsync("api/faqs");
                if (!res.IsSuccessStatusCode) return NotFound();

                var json = await res.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<FaqItem>>(json, _json) ?? new();
                var f = list.FirstOrDefault(x => x.Id == id);
                if (f == null) return NotFound();

                return View("FaqForm", new FaqFormViewModel
                {
                    Id = f.Id,
                    Category = f.Category,
                    Question = f.Question,
                    Answer = f.Answer
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading FAQ {Id}", id);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFaq(int id, FaqFormViewModel model)
        {
            if (!ModelState.IsValid) { model.Id = id; return View("FaqForm", model); }

            try
            {
                var payload = new { category = InputSanitizer.StripHtml(model.Category), question = InputSanitizer.StripHtml(model.Question), answer = InputSanitizer.StripHtml(model.Answer) };

                var client = _http.CreateClient("NodeApi");
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                await client.PutAsync($"api/faqs/{id}", content);

                return RedirectToAction(nameof(Faqs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FAQ {Id}", id);
                ModelState.AddModelError("", "Unable to update the FAQ.");
                model.Id = id;
                return View("FaqForm", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFaq(int id)
        {
            try
            {
                var client = _http.CreateClient("NodeApi");
                await client.DeleteAsync($"api/faqs/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting FAQ {Id}", id);
            }
            return RedirectToAction(nameof(Faqs));
        }

    }

    public class AdminUserRowViewModel
    {
        public string Id { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = "";
        public string? Branch { get; set; }
        public bool Active { get; set; }
    }
}