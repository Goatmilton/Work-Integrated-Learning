using System.Text.Json;
using SupabaseClient = Supabase.Client;
using PostgrestConstants = Supabase.Postgrest.Constants;
using Woodlands_Prototype_Insy7315.Models;

namespace Woodlands_Prototype_Insy7315.Services
{
    public class ProductService
    {
        private readonly SupabaseClient _supabase;

        public ProductService(SupabaseClient supabase)
        {
            _supabase = supabase;
        }

        public async Task<IReadOnlyList<Product>> GetProducts(
            CancellationToken cancellationToken = default)
        {
            var response = await _supabase
                .From<SupabaseProduct>()
                .Select("*")
                .Order("category", PostgrestConstants.Ordering.Ascending)
                .Order("title", PostgrestConstants.Ordering.Ascending)
                .Get(cancellationToken);

            return response.Models.Select(ToProduct).ToList();
        }

        public async Task<Product?> GetProduct(
            string id,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var response = await _supabase
                .From<SupabaseProduct>()
                .Where(p => p.Id == id)
                .Single(cancellationToken);

            return response == null ? null : ToProduct(response);
        }

        public async Task<Product> CreateProduct(
            Product product,
            CancellationToken cancellationToken = default)
        {
            Validate(product);

            var id = await CreateUniqueId(product.Title, cancellationToken);
            var record = ToSupabaseProduct(product, id);
            await _supabase
                .From<SupabaseProduct>()
                .Insert(record, cancellationToken: cancellationToken);

            return ToProduct(record);
        }

        public async Task<Product?> UpdateProduct(
            string id,
            Product product,
            CancellationToken cancellationToken = default)
        {
            Validate(product);

            var existing = await _supabase
                .From<SupabaseProduct>()
                .Where(p => p.Id == id)
                .Single(cancellationToken);

            if (existing == null)
                return null;

            var updated = ToSupabaseProduct(product, id);
            await _supabase
                .From<SupabaseProduct>()
                .Update(updated, cancellationToken: cancellationToken);

            return ToProduct(updated);
        }

        public async Task<bool> DeleteProduct(
            string id,
            CancellationToken cancellationToken = default)
        {
            var existing = await _supabase
                .From<SupabaseProduct>()
                .Where(p => p.Id == id)
                .Single(cancellationToken);

            if (existing == null)
                return false;

            await _supabase
                .From<SupabaseProduct>()
                .Delete(existing, cancellationToken: cancellationToken);

            return true;
        }

        private async Task<string> CreateUniqueId(
            string title,
            CancellationToken cancellationToken)
        {
            var baseId = new string(title
                .Trim()
                .ToLowerInvariant()
                .Select(character => char.IsLetterOrDigit(character) ? character : '-')
                .ToArray());
            baseId = string.Join("-", baseId.Split('-', StringSplitOptions.RemoveEmptyEntries));

            var id = string.IsNullOrWhiteSpace(baseId) ? Guid.NewGuid().ToString("N") : baseId;
            var candidate = id;
            var counter = 2;

            while (await GetProduct(candidate, cancellationToken) != null)
                candidate = $"{id}-{counter++}";

            return candidate;
        }

        private static void Validate(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            if (string.IsNullOrWhiteSpace(product.Category))
                throw new ArgumentException("Category is required.", nameof(product));

            if (string.IsNullOrWhiteSpace(product.Title))
                throw new ArgumentException("Title is required.", nameof(product));
        }

        private static Product ToProduct(SupabaseProduct product)
        {
            return new Product
            {
                Id = product.Id,
                Category = product.Category,
                Title = product.Title,
                Tagline = product.Tagline,
                Description = product.Description,
                Image = product.Image,
                GalleryJson = product.Gallery ?? "[]",
                FeaturesJson = product.Features ?? "[]",
                FinishesJson = product.Finishes ?? "[]",
                LeadTime = product.LeadTime,
                Tag = product.Tag,
                Price = product.Price
            };
        }

        private static SupabaseProduct ToSupabaseProduct(Product product, string id)
        {
            return new SupabaseProduct
            {
                Id = id,
                Category = product.Category.Trim(),
                Title = product.Title.Trim(),
                Tagline = product.Tagline,
                Description = product.Description,
                Image = product.Image,
                Gallery = NormalizeJson(product.GalleryJson),
                Features = NormalizeJson(product.FeaturesJson),
                Finishes = NormalizeJson(product.FinishesJson),
                LeadTime = product.LeadTime,
                Tag = product.Tag,
                Price = product.Price
            };
        }

        private static string NormalizeJson(string json)
        {
            try
            {
                return JsonSerializer.Serialize(
                    JsonSerializer.Deserialize<List<string>>(json ?? "[]") ?? new List<string>());
            }
            catch (JsonException)
            {
                throw new ArgumentException("Product list fields must contain valid JSON arrays.");
            }
        }
    }
}

// 