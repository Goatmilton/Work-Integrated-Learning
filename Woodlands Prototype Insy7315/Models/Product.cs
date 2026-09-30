using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Woodlands_Prototype_Insy7315.Models
{
    public class Product
    {
        [Key]
        [StringLength(100)]
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [Required]
        [StringLength(100)]
        [JsonPropertyName("category")]
        public string Category { get; set; } = "";

        [Required]
        [StringLength(150)]
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [StringLength(250)]
        [JsonPropertyName("tagline")]
        public string Tagline { get; set; } = "";

        [StringLength(2000)]
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [StringLength(500)]
        [JsonPropertyName("image")]
        public string Image { get; set; } = "";

        [JsonPropertyName("gallery")]
        public List<string> Gallery
        {
            get => ReadList(GalleryJson);
            set => GalleryJson = WriteList(value);
        }

        [JsonPropertyName("features")]
        public List<string> Features
        {
            get => ReadList(FeaturesJson);
            set => FeaturesJson = WriteList(value);
        }

        [JsonIgnore]
        public string GalleryJson { get; set; } = "[]";

        [JsonIgnore]
        public string FeaturesJson { get; set; } = "[]";

        [JsonIgnore]
        public string FinishesJson { get; set; } = "[]";

        [NotMapped]
        [JsonPropertyName("finishes")]
        public List<string> Finishes
        {
            get => ReadList(FinishesJson);
            set => FinishesJson = WriteList(value);
        }

        [StringLength(100)]
        [JsonPropertyName("lead_time")]
        public string LeadTime { get; set; } = "";

        [StringLength(50)]
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }

        [StringLength(50)]
        [JsonPropertyName("price")]
        public string? Price { get; set; }

        [JsonPropertyName("is_from_price")]
        public bool IsFromPrice { get; set; }

        [NotMapped]
        public string DisplayPrice
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Price))
                    return "Price on request";
                return IsFromPrice ? $"From {Price}" : Price;
            }
        }

        private static List<string> ReadList(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new List<string>();
            try
            {
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch
            {
                try
                {
                    var wrapped = JsonSerializer.Deserialize<string>(json);
                    if (wrapped != null)
                        return JsonSerializer.Deserialize<List<string>>(wrapped) ?? new List<string>();
                }
                catch { }
                return new List<string>();
            }
        }

        private static string WriteList(List<string>? values)
        {
            return JsonSerializer.Serialize(values ?? new List<string>());
        }
    }

    public class ProductCategory
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = "";
        [StringLength(100)]
        public string Label { get; set; } = "";
        [StringLength(500)]
        public string Image { get; set; } = "";
        public int Count { get; set; }
        [StringLength(1000)]
        public string Description { get; set; } = "";
        [StringLength(100)]
        public string Slug { get; set; } = "";
    }

    public class HeroSlide
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string Heading { get; set; } = "";
        [StringLength(100)]
        public string Accent { get; set; } = "";
        [StringLength(150)]
        public string Sub { get; set; } = "";
        [StringLength(500)]
        public string Image { get; set; } = "";
        [StringLength(50)]
        public string Cta { get; set; } = "";
        [StringLength(500)]
        public string Link { get; set; } = "";
    }
}