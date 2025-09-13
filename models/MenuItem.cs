using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TruYum.models;

namespace TruYum.Api.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(500)]
        public string Description { get; set; } = "";

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public bool Active { get; set; }
        public DateTime LaunchDate { get; set; } = DateTime.UtcNow;

        public string? ImageUrl { get; set; }

        public bool Veg { get; set; }

        // 🔹 Normalized Category
        public int CategoryId { get; set; }   // FK
        public Category? Category { get; set; } // Navigation property
    }
}