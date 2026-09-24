using SupabaseClient = Supabase.Client;
using Woodlands_Prototype_Insy7315.Models;

namespace Woodlands_Prototype_Insy7315.Services
{
    public class EnquiryService
    {
        private static readonly string[] AllowedStatuses =
        {
            "pending",
            "in progress",
            "completed",
            "cancelled"
        };

        private readonly SupabaseClient _supabase;

        public EnquiryService(SupabaseClient supabase)
        {
            _supabase = supabase;
        }

        /// <summary>
        /// Creates every enquiry in the pending state and generates its public reference.
        /// </summary>
        public async Task<SupabaseQuoteRequest> SubmitEnquiry(
            ContactRequest request,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(request);

            var quote = new SupabaseQuoteRequest
            {
                QuoteCode = GenerateQuoteCode(),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim(),
                Phone = request.Phone.Trim(),
                Branch = request.Branch.Trim(),
                Service = request.Service.Trim(),
                Message = request.Message.Trim(),
                ProductId = request.ProductId?.Trim() ?? "",
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };

            await _supabase
                .From<SupabaseQuoteRequest>()
                .Insert(quote, cancellationToken: cancellationToken);

            return quote;
        }

        /// <summary>
        /// Assigns an enquiry to a branch, which is the routing unit used by the dashboard.
        /// </summary>
        public async Task<SupabaseQuoteRequest?> AssignEnquiry(
            string identifier,
            string branch,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(branch))
                throw new ArgumentException("A target branch is required.", nameof(branch));

            var enquiry = await FindEnquiry(identifier, cancellationToken);
            if (enquiry == null)
                return null;

            enquiry.Branch = branch.Trim();
            await _supabase
                .From<SupabaseQuoteRequest>()
                .Update(enquiry, cancellationToken: cancellationToken);

            return enquiry;
        }

        /// <summary>
        /// Updates an enquiry only to one of the workflow statuses supported by the dashboard.
        /// </summary>
        public async Task<SupabaseQuoteRequest?> UpdateStatus(
            string identifier,
            string status,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(status))
                throw new ArgumentException("A status is required.", nameof(status));

            var normalizedStatus = status.Trim().ToLowerInvariant();
            if (!AllowedStatuses.Contains(normalizedStatus))
            {
                throw new ArgumentException(
                    $"Status must be one of: {string.Join(", ", AllowedStatuses)}.",
                    nameof(status));
            }

            var enquiry = await FindEnquiry(identifier, cancellationToken);
            if (enquiry == null)
                return null;

            enquiry.Status = normalizedStatus;
            await _supabase
                .From<SupabaseQuoteRequest>()
                .Update(enquiry, cancellationToken: cancellationToken);

            return enquiry;
        }

        private async Task<SupabaseQuoteRequest?> FindEnquiry(
            string identifier,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return null;

            var enquiry = await _supabase
                .From<SupabaseQuoteRequest>()
                .Where(q => q.QuoteCode == identifier)
                .Single(cancellationToken);

            if (enquiry != null || !long.TryParse(identifier, out var numericId))
                return enquiry;

            return await _supabase
                .From<SupabaseQuoteRequest>()
                .Where(q => q.Id == numericId)
                .Single(cancellationToken);
        }

        private static string GenerateQuoteCode()
        {
            return $"WL-{DateTime.UtcNow:yyyyMMddHHmmss}-" +
                   $"{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";
        }
    }
}