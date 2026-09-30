using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Woodlands_Prototype_Insy7315.Models
{
    public class FaqItem
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [JsonPropertyName("category")]
        public string Category { get; set; } = "";

        [Required]
        [StringLength(500)]
        [JsonPropertyName("question")]
        public string Question { get; set; } = "";

        [Required]
        [StringLength(2000)]
        [JsonPropertyName("answer")]
        public string Answer { get; set; } = "";
    }
}