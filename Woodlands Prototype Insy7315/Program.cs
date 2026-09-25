using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddHttpClient("NodeApi", client =>
{
    var baseUrl = builder.Configuration["NodeApi:BaseUrl"] ?? "http://localhost:5000/";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<SupabaseAuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAndroidApp", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "WoodlandsCookie";
        options.DefaultChallengeScheme = "WoodlandsCookie";
        options.DefaultSignInScheme = "WoodlandsCookie";
    })
    .AddCookie("WoodlandsCookie", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

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