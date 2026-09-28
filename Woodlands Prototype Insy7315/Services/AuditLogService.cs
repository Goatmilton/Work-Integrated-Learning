using Microsoft.AspNetCore.Http;
using Woodlands_Prototype_Insy7315.Data;
using Woodlands_Prototype_Insy7315.Models;

namespace Woodlands_Prototype_Insy7315.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(string eventType, bool success, string? subject = null, string? userId = null, string? details = null);
    }

    // Writes a row to AuditLogs for every security-relevant event: login
    // attempts (success and failure), registrations, role/permission
    // changes, and access-denied events. Deliberately fire-and-forget-safe:
    // a logging failure is written to the app log but never blocks the
    // request or bubbles up to the user.
    public class AuditLogService : IAuditLogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuditLogService> _logger;

        public AuditLogService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuditLogService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task LogAsync(string eventType, bool success, string? subject = null, string? userId = null, string? details = null)
        {
            try
            {
                var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                var entry = new AuditLog
                {
                    EventType = eventType,
                    Success = success,
                    Subject = Truncate(subject, 256),
                    UserId = Truncate(userId, 450),
                    Details = Truncate(details, 1000),
                    IpAddress = Truncate(ip, 45),
                    CreatedAtUtc = DateTime.UtcNow
                };

                _context.AuditLogs.Add(entry);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Auditing must never take down the request pipeline.
                _logger.LogError(ex, "Failed to write audit log entry for {EventType}", eventType);
            }
        }

        private static string? Truncate(string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value[..maxLength];
        }
    }
}
