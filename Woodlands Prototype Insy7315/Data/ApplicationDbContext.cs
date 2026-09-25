using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Woodlands_Prototype_Insy7315.Models;

namespace Woodlands_Prototype_Insy7315.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<HeroSlide> HeroSlides => Set<HeroSlide>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<QuoteRequest> QuoteRequests => Set<QuoteRequest>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Testimonial> Testimonials => Set<Testimonial>();
        public DbSet<FaqItem> Faqs => Set<FaqItem>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<QuoteRequest>()
                .HasOne(q => q.BranchNavigation)
                .WithMany(b => b.QuoteRequests)
                .HasForeignKey(q => q.BranchId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<QuoteRequest>()
                .HasOne(q => q.AppUser)
                .WithMany(u => u.QuoteRequests)
                .HasForeignKey(q => q.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Testimonial>()
                .HasOne(t => t.AppUser)
                .WithMany(u => u.Testimonials)
                .HasForeignKey(t => t.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Branch>().HasData(
                new Branch { Id = 1, Name = "Main HQ", Region = "Gauteng", Phone = "011-555-0000" },
                new Branch { Id = 2, Name = "Coastal Branch", Region = "Western Cape", Phone = "021-555-0000" }
            );

            builder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Custom Design", Description = "Bespoke design services", IsActive = true },
                new Service { Id = 2, Name = "Installation", Description = "Professional installation", IsActive = true }
            );
        }
    }
}