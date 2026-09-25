using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Woodlands_Prototype_Insy7315.Models
{
    public class Testimonial
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [StringLength(100)]
        [JsonPropertyName("role")]
        public string Role { get; set; } = "";

        [StringLength(100)]
        [JsonPropertyName("location")]
        public string Location { get; set; } = "";

        [Range(1, 5)]
        [JsonPropertyName("rating")]
        public int Rating { get; set; } = 5;

        [Required]
        [StringLength(1500)]
        [JsonPropertyName("review")]
        public string Review { get; set; } = "";

        [StringLength(200)]
        [JsonPropertyName("project")]
        public string Project { get; set; } = "";

        public string? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser? AppUser { get; set; }
    }
}