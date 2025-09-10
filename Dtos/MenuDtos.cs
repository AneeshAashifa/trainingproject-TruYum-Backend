namespace TruYum.Api.Dtos
{
    // Base DTO for returning menu items
    public record MenuItemDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        bool Active,
        DateTime LaunchDate,
        string? ImageUrl,
        bool Veg,
        string Category
    );

    // DTO for creating/updating items
    public record MenuItemCreateDto(
        string Name,
        string Description,
        decimal Price,
        bool Active,
        string? ImageUrl,
        bool Veg,
        string Category   // "Beverage", "Starter", "MainCourse", "Snack"
    );
}