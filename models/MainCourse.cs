namespace TruYum.Api.Models
{
    public class MainCourse : MenuItem
    {
        public string? Cuisine { get; set; }
        public string? Size { get; set; } // e.g., Small/Medium/Large
    }
}

