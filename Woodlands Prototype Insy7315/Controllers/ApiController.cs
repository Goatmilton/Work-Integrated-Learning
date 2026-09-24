using Microsoft.AspNetCore.Mvc;
<<<<<<< Updated upstream
using Supabase.Gotrue;
using Woodlands_Prototype_Insy7315.Models;
=======
using Microsoft.AspNetCore.Authorization;
using Supabase.Gotrue;
using Woodlands_Prototype_Insy7315.Models;
using Woodlands_Prototype_Insy7315.Services;
>>>>>>> Stashed changes
using static Supabase.Gotrue.Constants;
using PostgrestConstants = Supabase.Postgrest.Constants;
using SupabaseClient = Supabase.Client;

namespace Woodlands_Prototype_Insy7315.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly SupabaseClient _supabase;
<<<<<<< Updated upstream
=======
        private readonly EnquiryService _enquiryService;
        private readonly ProductService _productService;
>>>>>>> Stashed changes
        private readonly ILogger<ApiController> _logger;

        public ApiController(
            SupabaseClient supabase,
<<<<<<< Updated upstream
            ILogger<ApiController> logger)
        {
            _supabase = supabase;
=======
            EnquiryService enquiryService,
            ProductService productService,
            ILogger<ApiController> logger)
        {
            _supabase = supabase;
            _enquiryService = enquiryService;
            _productService = productService;
>>>>>>> Stashed changes
            _logger = logger;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
<<<<<<< Updated upstream
                var response = await _supabase
                    .From<SupabaseProduct>()
                    .Select("*")
                    .Order(
                        "category",
                        PostgrestConstants.Ordering.Ascending)
                    .Order(
                        "title",
                        PostgrestConstants.Ordering.Ascending)
                    .Get();

                return Ok(response.Models);
=======
                return Ok(await _productService.GetProducts());
>>>>>>> Stashed changes
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
<<<<<<< Updated upstream
                var response = await _supabase
                    .From<SupabaseProduct>()
                    .Where(p => p.Id == id)
                    .Single();

                if (response == null)
                    return NotFound();

                return Ok(response);
=======
                var product = await _productService.GetProduct(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
>>>>>>> Stashed changes
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("products/category/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            try
            {
                var response = await _supabase
                    .From<SupabaseProduct>()
                    .Where(p => p.Category == category)
                    .Order(
                        "title",
                        PostgrestConstants.Ordering.Ascending)
                    .Get();

                return Ok(response.Models);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error fetching products by category");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpGet("testimonials")]
        public async Task<IActionResult> GetTestimonials()
        {
            try
            {
                var response = await _supabase
                    .From<SupabaseTestimonial>()
                    .Select("*")
                    .Order(
                        "id",
                        PostgrestConstants.Ordering.Ascending)
                    .Get();

                return Ok(response.Models);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error fetching testimonials");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpGet("faqs")]
        public async Task<IActionResult> GetFaqs()
        {
            try
            {
                var response = await _supabase
                    .From<SupabaseFaqItem>()
                    .Select("*")
                    .Order(
                        "category",
                        PostgrestConstants.Ordering.Ascending)
                    .Get();

                return Ok(response.Models);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching FAQs");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpGet("faqs/{category}")]
        public async Task<IActionResult> GetFaqsByCategory(
            string category)
        {
            try
            {
                var response = await _supabase
                    .From<SupabaseFaqItem>()
                    .Where(f => f.Category == category)
                    .Order(
                        "id",
                        PostgrestConstants.Ordering.Ascending)
                    .Get();

                return Ok(response.Models);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error fetching FAQs by category");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                var response = await _supabase
                    .From<SupabaseService>()
                    .Where(s => s.IsActive)
                    .Order(
                        "id",
                        PostgrestConstants.Ordering.Ascending)
                    .Get();

                return Ok(response.Models);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching services");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpGet("branches")]
        public async Task<IActionResult> GetBranches()
        {
            try
            {
                var response = await _supabase
                    .From<SupabaseBranch>()
                    .Select("*")
                    .Order(
                        "id",
                        PostgrestConstants.Ordering.Ascending)
                    .Get();

                return Ok(response.Models);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching branches");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpGet("homepage-assets")]
        public async Task<IActionResult> GetHomepageAssets()
        {
            try
            {
                var response = await _supabase
                    .From<SupabaseHomepageAsset>()
                    .Where(a => a.IsActive)
                    .Order(
                        "display_order",
                        PostgrestConstants.Ordering.Ascending)
                    .Get();

                return Ok(response.Models);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error fetching homepage assets");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpPost("quote-requests")]
        public async Task<IActionResult> CreateQuoteRequest(
            [FromBody] CreateQuoteRequestDto dto)
        {
            try
            {
<<<<<<< Updated upstream
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var quoteCode =
                    $"WL-{DateTime.UtcNow:yyyyMMddHHmmss}-" +
                    $"{Guid.NewGuid().ToString()[..8].ToUpper()}";

                var quoteRequest = new SupabaseQuoteRequest
                {
                    QuoteCode = quoteCode,
=======
                var request = new ContactRequest
                {
>>>>>>> Stashed changes
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Branch = dto.Branch,
                    Service = dto.Service,
                    Message = dto.Message,
<<<<<<< Updated upstream
                    ProductId = dto.ProductId,
                    Status = "pending",
                    CreatedAt = DateTime.UtcNow
                };

                await _supabase
                    .From<SupabaseQuoteRequest>()
                    .Insert(quoteRequest);

                return Ok(new
                {
                    quoteCode,
=======
                    ProductId = dto.ProductId
                };

                if (!TryValidateModel(request))
                    return ValidationProblem(ModelState);

                var quote = await _enquiryService.SubmitEnquiry(request);

                return Ok(new
                {
                    quoteCode = quote.QuoteCode,
>>>>>>> Stashed changes
                    message = "Quote request created successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating quote request");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

<<<<<<< Updated upstream
=======
        [HttpPost("enquiries")]
        public async Task<IActionResult> SubmitEnquiry(
            [FromBody] ContactRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var enquiry = await _enquiryService.SubmitEnquiry(request);
                return Created($"api/enquiries/{enquiry.QuoteCode}", new
                {
                    quoteCode = enquiry.QuoteCode,
                    status = enquiry.Status,
                    message = "Enquiry submitted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting enquiry");
                return StatusCode(500, new { error = "Unable to submit enquiry." });
            }
        }

        [Authorize(Roles = "Admin,Manager (Soweto),Manager (Roodepoort),Manager (Randfontein)")]
        [HttpPatch("enquiries/{identifier}/assignment")]
        public async Task<IActionResult> AssignEnquiry(
            string identifier,
            [FromBody] AssignmentDto dto)
        {
            try
            {
                var enquiry = await _enquiryService.AssignEnquiry(identifier, dto.Branch);
                return enquiry == null ? NotFound() : Ok(enquiry);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Manager (Soweto),Manager (Roodepoort),Manager (Randfontein)")]
        [HttpPatch("enquiries/{identifier}/status")]
        public async Task<IActionResult> UpdateEnquiryStatus(
            string identifier,
            [FromBody] StatusDto dto)
        {
            try
            {
                var enquiry = await _enquiryService.UpdateStatus(identifier, dto.Status);
                return enquiry == null ? NotFound() : Ok(enquiry);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct(
            [FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var created = await _productService.CreateProduct(product);
                return CreatedAtAction(
                    nameof(GetProduct),
                    new { id = created.Id },
                    created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateProduct(
            string id,
            [FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var updated = await _productService.UpdateProduct(id, product);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            return await _productService.DeleteProduct(id)
                ? NoContent()
                : NotFound();
        }

>>>>>>> Stashed changes
        [HttpPost("auth/register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDto dto)
        {
            try
            {
                var session = await _supabase.Auth.SignUp(
                    email: dto.Email,
                    password: dto.Password);

                if (session?.User == null)
                    return BadRequest("Registration failed");

                var appUser = new SupabaseAppUser
                {
                    Id = session!.User!.Id.ToString(),
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Role = "Customer",
                    Active = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _supabase
                    .From<SupabaseAppUser>()
                    .Insert(appUser);

                return Ok(new
                {
                    message =
                        "Registration successful. Check your email."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error during registration");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto dto)
        {
            try
            {
                var session =
                    await _supabase.Auth.SignInWithPassword(
                        dto.Email,
                        dto.Password);

                if (session == null)
                    return Unauthorized("Invalid credentials");

                return Ok(new
                {
                    accessToken = session.AccessToken,
                    refreshToken = session.RefreshToken,
                    expiresIn = session.ExpiresIn
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error during login");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        [HttpPost("auth/refresh")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenDto dto)
        {
            try
            {
                var session = await _supabase.Auth.SignIn(
                    SignInType.RefreshToken,
                    dto.RefreshToken);

                if (session == null)
                    return Unauthorized(
                        "Token refresh failed");

                return Ok(new
                {
                    accessToken = session.AccessToken,
                    refreshToken = session.RefreshToken,
                    expiresIn = session.ExpiresIn
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error refreshing token");

                return StatusCode(
                    500,
                    new { error = ex.Message });
            }
        }

        public class RegisterDto
        {
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
            public string Phone { get; set; } = "";
        }

        public class LoginDto
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public class RefreshTokenDto
        {
            public string RefreshToken { get; set; } = "";
        }

        public class CreateQuoteRequestDto
        {
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Branch { get; set; } = "";
            public string Service { get; set; } = "";
            public string Message { get; set; } = "";
            public string ProductId { get; set; } = "";
        }
<<<<<<< Updated upstream
=======

        public class AssignmentDto
        {
            public string Branch { get; set; } = "";
        }

        public class StatusDto
        {
            public string Status { get; set; } = "";
        }
>>>>>>> Stashed changes
    }
}