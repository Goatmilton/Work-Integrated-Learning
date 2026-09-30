using System.ComponentModel.DataAnnotations;

namespace Woodlands_Prototype_Insy7315.Models
{
    // Immutable record of a security-relevant event (login, registration,
    // role change, access denial, etc). Written by IAuditLogService and
    // never updated after creation.
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string EventType { get; set; } = "";

        // Email or identifier the event relates to. Not a foreign key so
        // that failed-login attempts against unknown accounts are still
        // captured.
        [StringLength(256)]
        public string? Subject { get; set; }

        [StringLength(450)]
        public string? UserId { get; set; }

        public bool Success { get; set; }

        [StringLength(1000)]
        public string? Details { get; set; }

        [StringLength(45)]
        public string? IpAddress { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
