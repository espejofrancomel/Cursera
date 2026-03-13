using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string FirstName { get; set; } = default!;

        [Required, StringLength(100)]
        public string LastName { get; set; } = default!;

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; } = default!;

        [StringLength(100)]
        public string? Department { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}