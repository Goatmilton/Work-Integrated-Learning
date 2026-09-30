using System.Text.RegularExpressions;

namespace Woodlands_Prototype_Insy7315.Services
{
    // Defense-in-depth against stored XSS for free-text fields (testimonials,
    // FAQs, contact messages) that get persisted and later rendered back to
    // other users. Razor already HTML-encodes on output by default, so this
    // is a second layer: strip markup at the point of entry so raw HTML/script
    // never reaches the database at all.
    public static class InputSanitizer
    {
        private static readonly Regex TagPattern = new("<[^>]*>", RegexOptions.Compiled);

        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(input))]
        public static string? StripHtml(string? input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var withoutTags = TagPattern.Replace(input, string.Empty);
            return withoutTags
                .Replace("javascript:", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Trim();
        }
    }
}
