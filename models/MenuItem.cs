using System.ComponentModel.DataAnnotations;

namespace TruYum.Api.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1024)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal Price { get; set; }

        public bool Active { get; set; } = true;
        public DateTime LaunchDate { get; set; } = DateTime.UtcNow;

        public string? ImageUrl { get; set; }
        public bool Veg { get; set; } = true;

        // ✅ NEW FIELD (instead of different tables)
        [Required, MaxLength(50)]
        public string Category { get; set; } = string.Empty;
    }
}