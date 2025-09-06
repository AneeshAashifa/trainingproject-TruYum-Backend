using System.ComponentModel.DataAnnotations;

namespace TruYum.Api.Models
{
    public abstract class MenuItem
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

        // Optional common media fields
        public string? ImageUrl { get; set; }
        public bool Veg { get; set; } = true;

        // NOTE: Category is implied by the derived table (Beverage/Starter/MainCourse/Snack)
    }
}

