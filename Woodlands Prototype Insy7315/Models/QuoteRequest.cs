using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Woodlands_Prototype_Insy7315.Models
{
    public class QuoteRequest
    {
        [Key]
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(50)]
        [JsonPropertyName("quote_code")]
        public string QuoteCode { get; set; } = "";

        [Required]
        [StringLength(100)]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(100)]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [Phone]
        [StringLength(20)]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        // Retaining the string for frontend JSON, mapping the real ID for the database
        [JsonPropertyName("branch")]
        public string? BranchName { get; set; }

        public long? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch? BranchNavigation { get; set; }

        [StringLength(100)]
        [JsonPropertyName("service")]
        public string? Service { get; set; }

        [StringLength(1500)]
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [StringLength(100)]
        [JsonPropertyName("product_id")]
        public string? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [StringLength(50)]
        [JsonPropertyName("status")]
        public string Status { get; set; } = "Pending";

        [StringLength(100)]
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser? AppUser { get; set; }
    }
}