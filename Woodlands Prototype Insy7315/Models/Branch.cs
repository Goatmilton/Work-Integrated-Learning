using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Woodlands_Prototype_Insy7315.Models
{
    public class Branch
    {
        [Key]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [Required]
        [StringLength(150)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [StringLength(100)]
        [JsonPropertyName("region")]
        public string Region { get; set; } = "";

        [Phone]
        [StringLength(20)]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [StringLength(100)]
        [JsonPropertyName("hours")]
        public string? Hours { get; set; }

        [StringLength(500)]
        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<QuoteRequest> QuoteRequests { get; set; } = new List<QuoteRequest>();
    }
}