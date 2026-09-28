using System.ComponentModel.DataAnnotations;

namespace Woodlands_Prototype_Insy7315.Models
{
    public class UserFormViewModel
    {
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; } = IdentitySeederRoles.Customer;

        public string? Branch { get; set; }

        public bool Active { get; set; } = true;

        [DataType(DataType.Password)]
        [MinLength(8)]
        public string? Password { get; set; }
    }

    // The four canonical RBAC roles seeded by DbInitializer. Branch is a
    // separate attribute on the user (UserFormViewModel.Branch / ApplicationUser.Branch)
    // rather than folded into the role name, so a Manager's role check works
    // the same way regardless of which branch they're assigned to.
    public static class IdentitySeederRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Sales = "Sales";
        public const string Customer = "Customer";

        public static readonly string[] All =
        {
            Admin,
            Manager,
            Sales,
            Customer
        };
    }
}
