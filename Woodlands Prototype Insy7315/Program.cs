using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Woodlands_Prototype_Insy7315.Data;
using Woodlands_Prototype_Insy7315.Models; 
using Woodlands_Prototype_Insy7315.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.ConfigureWarnings(warnings =>
        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Password policy - enforced server-side on register, admin create
        // and any password change. Passwords are stored as PBKDF2 hashes.
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;

        // Brute-force protection: lock the account after 5 failed attempts.
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Browser sessions use Identity's application cookie (this is the scheme
// SignInManager signs into, so it must not be replaced by a custom one).
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddHttpClient("NodeApi", client =>
{
    var baseUrl = builder.Configuration["NodeApi:BaseUrl"] ?? "http://localhost:5000/";
    client.BaseAddress = new Uri(baseUrl);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAndroidApp", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


// JWT bearer is added as a second scheme for API clients. It does not
// replace the cookie default, so MVC pages keep using the Identity cookie
// and API controllers opt in with [Authorize(AuthenticationSchemes = "Bearer")].
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key is missing or shorter than 32 characters. Set it with: " +
        "dotnet user-secrets set \"Jwt:Key\" \"<random 32+ character string>\"");
}

builder.Services
    .AddAuthentication()
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "WoodlandsApi",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "WoodlandsClients",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StaffOnly", policy =>
        policy.RequireRole(IdentitySeederRoles.Admin, IdentitySeederRoles.Manager, IdentitySeederRoles.Sales));
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(IdentitySeederRoles.Admin));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DbInitializer.SeedAsync(services);
        Console.WriteLine("--> Azure SQL Database Seeded Successfully with WoodLinkData.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Error seeding database: {ex.Message}");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAndroidApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();