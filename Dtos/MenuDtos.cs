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
        int CategoryId, 
        string CategoryName
    );

    // DTO for creating/updating items
    public record MenuItemCreateDto(
        string Name,
        string Description,
        decimal Price,
        bool Active,
        string? ImageUrl,
        bool Veg,
        int CategoryId   
    );
}