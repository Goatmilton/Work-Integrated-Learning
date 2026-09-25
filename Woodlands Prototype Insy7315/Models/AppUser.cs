using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Woodlands_Prototype_Insy7315.Models
{
    public class AppUser
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = "";

        [Required]
        [EmailAddress]
        [StringLength(150)]
        [JsonPropertyName("email")]
        public string Email { get; set; } = "";

        [Phone]
        [StringLength(20)]
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = "";

        [Required]
        [StringLength(50)]
        [JsonPropertyName("role")]
        public string Role { get; set; } = "Customer";

        [StringLength(100)]
        [JsonPropertyName("branch")]
        public string? Branch { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties for EF Core relationship mapping
        public ICollection<QuoteRequest> QuoteRequests { get; set; } = new List<QuoteRequest>();
        public ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
    }
}